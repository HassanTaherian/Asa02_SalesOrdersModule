using Contracts.Marketing.Send;
using Contracts.Product;
using Contracts.UI;
using Contracts.UI.Checkout;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.ValueObjects;
using Services.Abstractions;
using Services.External;

namespace Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IHttpProvider _httpProvider;

        public OrderService(IInvoiceRepository invoiceRepository, IHttpProvider httpProvider)
        {
            _invoiceRepository = invoiceRepository;
            _httpProvider = httpProvider;
        }

        public async Task Checkout(CheckoutRequestDto dto)
        {
            var cart = await _invoiceRepository.GetCartOfUser(dto.UserId);

            if (cart.AddressId is null)
            {
                throw new AddressNotSpecifiedException(cart.UserId);
            }

            if (!CartHasItem(cart))
            {
                throw new EmptyCartException(dto.UserId);
            }

            var notDeletedItems = await _invoiceRepository.GetNotDeleteItems(cart.Id);

            await UpdateCountingOfProduct(notDeletedItems, ProductCountingState.ShopState);
            await SendInvoiceToMarketing(cart, InvoiceState.OrderState);

            await _invoiceRepository.ChangeInvoiceState(dto.UserId, InvoiceState.OrderState);
            cart.ShoppingDateTime = DateTime.Now;
            _invoiceRepository.UpdateInvoice(cart);
            await _invoiceRepository.SaveChangesAsync();
        }

        public async Task Returning(ReturningRequestDto returningRequestDto)
        {
            var invoice = await _invoiceRepository.GetInvoiceById(returningRequestDto.InvoiceId);

            switch (invoice.State)
            {
                case InvoiceState.ReturnState:
                    throw new AlreadyReturnedException(returningRequestDto.InvoiceId);
                case InvoiceState.CartState:
                    throw new InvoiceIsInCartStateException(returningRequestDto.InvoiceId);
            }

            var invoiceItems = new List<InvoiceItem>();

            foreach (var id in returningRequestDto.ProductIds)
            {
                var invoiceItem = await _invoiceRepository.GetInvoiceItem(returningRequestDto.InvoiceId, id);

                if (invoiceItem.IsDeleted)
                {
                    throw new InvoiceItemNotFoundException(invoice.Id, id);
                }
                invoiceItem.IsReturn = true;
                invoiceItems.Add(invoiceItem);
            }

            await UpdateCountingOfProduct(invoiceItems, ProductCountingState.ReturnState);
            await SendInvoiceToMarketing(invoice, InvoiceState.ReturnState);

            invoice.ReturnDateTime = DateTime.Now;
            invoice.State = InvoiceState.ReturnState;

            _invoiceRepository.UpdateInvoice(invoice);

            await _invoiceRepository.SaveChangesAsync();
        }

        // Todo: Make Private
        public async Task<bool> UpdateCountingOfProduct(IEnumerable<InvoiceItem> items, ProductCountingState state)
        {
            var countingDtos = MapInvoiceConfig(items, state);
            var jsonBridge = new JsonBridge<ProductUpdateCountingItemRequestDto, Boolean>();
            var json = jsonBridge.SerializeList(countingDtos.ToList());
            await _httpProvider.Post("https://localhost:7083/mock/DiscountMock/UpdateProductCounting", json);
            return true;
        }

        // Todo: Make Private
        public async Task<bool> SendInvoiceToMarketing(Invoice invoice, InvoiceState state)
        {
            var marketingInvoiceRequest = new MarketingInvoiceRequest
            {
                InvoiceId = invoice.Id,
                UserId = invoice.UserId,
                InvoiceState = state,
                ShopDateTime = invoice.ShoppingDateTime
            };

            var jsonBridge = new JsonBridge<MarketingInvoiceRequest, Boolean>();
            var json = jsonBridge.Serialize(marketingInvoiceRequest);
            await _httpProvider.Post("https://localhost:7083/mock/DiscountMock/Marketing", json);
            return true;
        }

        // Todo: Make Private
        private ICollection<ProductUpdateCountingItemRequestDto> MapInvoiceConfig(IEnumerable<InvoiceItem> invoiceItems,
            ProductCountingState state)
        {
            var countingDtos = new List<ProductUpdateCountingItemRequestDto>();

            foreach (var invoiceItem in invoiceItems)
            {
                var dto = new ProductUpdateCountingItemRequestDto()
                {
                    ProductId = invoiceItem.ProductId,
                    ProductCountingState = state,
                    Quantity = invoiceItem.Quantity
                };
                countingDtos.Add(dto);
            }

            return countingDtos;
        }

        // Todo: Check this after merging with CartService
        private bool CartHasItem(Invoice cart)
        {
            return cart.InvoiceItems.Any(invoiceItem => invoiceItem.IsDeleted == false);
        }
    }
}