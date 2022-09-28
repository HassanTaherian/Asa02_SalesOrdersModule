using Contracts.Marketing.Send;
using Contracts.Product;
using Contracts.UI;
using Contracts.UI.Checkout;
using Contracts.UI.Watch;
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
            var cart = _invoiceRepository.GetCartOfUser(dto.UserId);

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

            ChangeCartStateToOrderState(dto.UserId);
            cart.ShoppingDateTime = DateTime.Now;
            _invoiceRepository.UpdateInvoice(cart);
            await _invoiceRepository.SaveChangesAsync();
        }
        
        private void ChangeCartStateToOrderState(int userId)
        {
            var cart = _invoiceRepository.GetCartOfUser(userId);

            cart.State = InvoiceState.OrderState;
            _invoiceRepository.UpdateInvoice(cart);
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
                var invoiceItem = await _invoiceRepository.GetProductOfInvoice(returningRequestDto.InvoiceId, id);

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
        
               public List<WatchInvoicesResponseDto> OrderInvoices(WatchRequestItemsDto watchRequestItemsDto)
        {
            var invoices = _invoiceRepository.GetInvoiceByState(watchRequestItemsDto.UserId, InvoiceState.OrderState);
            if (invoices == null)
            {
                throw new InvoiceNotFoundException(watchRequestItemsDto.UserId);
            }

            return MapWatchOrderDto(invoices);

        }

        private static List<WatchInvoicesResponseDto> MapWatchOrderDto(IEnumerable<Invoice> invoices)
        {
            return invoices.Select(invoice => new WatchInvoicesResponseDto
            {
                InvoiceId = invoice.Id,
                DateTime = (DateTime)invoice.ShoppingDateTime
            })
                .ToList();
        }

        public List<WatchInvoicesResponseDto> ReturnInvoices(WatchRequestItemsDto watchRequestItemsDto)
        {
            var invoices = _invoiceRepository.GetInvoiceByState(watchRequestItemsDto.UserId, InvoiceState.ReturnState);
            if (invoices == null)
            {
                throw new InvoiceNotFoundException(watchRequestItemsDto.UserId);
            }

            return MapWatchReturnDto(invoices);

        }

        private static List<WatchInvoicesResponseDto> MapWatchReturnDto(IEnumerable<Invoice> invoices)
        {
            return invoices.Select(invoice => new WatchInvoicesResponseDto
            {
                InvoiceId = invoice.Id,
                DateTime = (((DateTime)invoice.ReturnDateTime)),
            })
                .ToList();
        }

        public async Task<List<WatchInvoiceItemsResponseDto>> ShoppedInvoiceItems(WatchInvoicesRequestDto watchInvoicesRequestDto)
        {
            var invoiceItems = await _invoiceRepository.GetNotDeleteItems(watchInvoicesRequestDto.InvoiceId);
            if (invoiceItems == null)
            {
                throw new EmptyInvoiceException(watchInvoicesRequestDto.InvoiceId);
            }

            // return MapWatchCartItemDto(invoiceItems);
            return null;
        }

        public async Task<List<WatchInvoiceItemsResponseDto>> ReturnedInvoiceItems(WatchInvoicesRequestDto watchInvoicesRequestDto)
        {
            var invoice = await _invoiceRepository.GetInvoiceById(watchInvoicesRequestDto.InvoiceId);

            var invoiceItems = 
                (from invoiceItem in invoice.InvoiceItems 
                    where invoiceItem.IsReturn == true 
                    select new WatchInvoiceItemsResponseDto 
                { ProductId = invoiceItem.ProductId,
                    Quantity = invoiceItem.Quantity,
                    UnitPrice = invoiceItem.Price })
                .ToList();

            if (invoiceItems == null)
            {
                throw new EmptyInvoiceException(watchInvoicesRequestDto.InvoiceId);
            }

            return invoiceItems;
        }
    }
}