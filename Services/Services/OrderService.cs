using Contracts.Marketing.Send;
using Contracts.Product;
using Contracts.UI;
using Contracts.UI.Checkout;
using Domain.Entities;
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

        public async Task<bool> Checkout(CheckoutRequestDto dto)
        {
            var cart = await _invoiceRepository.GetCartOfUser(dto.UserId);

            if (cart is null)
            {
                return false;
            }

            await UpdateCountingOfProduct(cart.InvoiceItems, ProductCountingState.ShopState);
            await SendInvoiceToMarketing(cart, InvoiceState.OrderState);

            var result = await _invoiceRepository.ChangeInvoiceState(dto.UserId, InvoiceState.OrderState);
            cart.ShoppingDateTime = DateTime.Now;
            _invoiceRepository.UpdateInvoice(cart);
            await _invoiceRepository.SaveChangesAsync();
            return result;
        }

        public async Task<bool> Returning(ReturningRequestDto returningRequestDto)
        {
            var invoice = await _invoiceRepository.GetInvoiceById(returningRequestDto.InvoiceId);
            var invoiceItems = new List<InvoiceItem>();
            foreach (var id in returningRequestDto.ProductIds)
            {
                var invoiceItem = await _invoiceRepository.GetInvoiceItem(returningRequestDto.InvoiceId, id);
                invoiceItem.IsReturn = true;
                invoiceItems.Add(invoiceItem);
            }

            await UpdateCountingOfProduct(invoiceItems, ProductCountingState.ReturnState);
            await SendInvoiceToMarketing(invoice, InvoiceState.ReturnState);

            invoice.ReturnDateTime = DateTime.Now;
            invoice.State = InvoiceState.ReturnState;

            _invoiceRepository.UpdateInvoice(invoice);

            var result = await _invoiceRepository.ChangeInvoiceState(invoice.UserId, InvoiceState.OrderState);
            await _invoiceRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCountingOfProduct(IEnumerable<InvoiceItem> items, ProductCountingState state)
        {
            var countingDtos = MapInvoiceConfig(items, state);
            var jsonBridge = new JsonBridge<ProductUpdateCountingItemRequestDto, Boolean>();
            var json = jsonBridge.SerializeList(countingDtos.ToList());
            await _httpProvider.Post("https://localhost:7083/mock/DiscountMock/UpdateProductCounting", json);
            return true;
        }

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
    }
}