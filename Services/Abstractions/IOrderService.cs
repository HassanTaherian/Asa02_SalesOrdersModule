using Contracts.UI.Checkout;
using Domain.Entities;
using Domain.ValueObjects;

namespace Services.Abstractions
{
    public interface IOrderService
    {
        Task<bool> Checkout(CheckoutRequestDto dto);

        Task<bool> DecreaseCountingOfProduct(IEnumerable<InvoiceItem> items, ProductCountingState state);

        Task<bool> SendInvoiceToMarketing(Invoice invoice, InvoiceState state);
    }
}
