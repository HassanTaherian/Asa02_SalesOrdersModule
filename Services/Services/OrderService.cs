using Contracts.Marketing.Send;
using Contracts.Product;
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
        private readonly HttpProvider _httpProvider;

        public OrderService(IInvoiceRepository invoiceRepository, HttpProvider httpProvider)
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

            await DecreaseCountingOfProduct(cart.InvoiceItems, ProductCountingState.ShopState);
            await SendInvoiceToMarketing(cart, InvoiceState.OrderState);

            var result = await _invoiceRepository.ChangeInvoiceState(dto.UserId, InvoiceState.OrderState);
            return result;
        }

        public async Task<bool> DecreaseCountingOfProduct(IEnumerable<InvoiceItem> items, ProductCountingState state)
        {
            var countingDtos = MapInvoiceConfig(items, state);
            var jsonBridge = new JsonBridge<ProductUpdateCountingItemRequestDto>();
            var json = jsonBridge.SerializeList(countingDtos);
            await _httpProvider.Post("url", json);
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

            var jsonBridge = new JsonBridge<MarketingInvoiceRequest>();
            var json = jsonBridge.Serialize(marketingInvoiceRequest);
            await _httpProvider.Post("marketingUrl", json);
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