using Contracts.Product;
using Contracts.UI.Checkout;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Services.Abstractions;

namespace Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public OrderService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<bool> Checkout(CheckoutRequestDto dto)
        {
            var result = await _invoiceRepository.ChangeInvoiceState(dto.UserId, InvoiceState.OrderState);

            var invoice = await _invoiceRepository.GetItemsOfInvoice(dto.UserId);
            var countingDtos = MapInvoiceConfig(invoice, ProductCountingState.ShopState);

            // Todo: Call ProductAdapter

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
