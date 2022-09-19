using Contracts.UI.Checkout;
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
            return await _invoiceRepository.ChangeInvoiceState(dto.UserId, InvoiceState.OrderState);
        }
    }
}
