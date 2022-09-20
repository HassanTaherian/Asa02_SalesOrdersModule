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
            var result = await _invoiceRepository.ChangeInvoiceState(dto.UserId, InvoiceState.OrderState);

            var invoice = await _invoiceRepository.GetItemsOfInvoice(dto.UserId);
            var countingDtos = MapInvoiceConfig(invoice, ProductCountingState.ShopState);

            var jsonBridge = new JsonBridge<ProductUpdateCountingItemRequestDto>();
            var json = jsonBridge.SerializeList(countingDtos);
            await _httpProvider.Post("url", json);

            return true;
        }

        private ICollection<ProductUpdateCountingItemRequestDto> MapInvoiceConfig(IEnumerable<InvoiceItem> invoiceItems, ProductCountingState state)
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
