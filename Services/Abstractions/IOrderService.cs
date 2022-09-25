using Contracts.UI;
using Contracts.UI.Checkout;
using Domain.Entities;
using Domain.ValueObjects;

namespace Services.Abstractions
{
    public interface IOrderService
    {
        Task Checkout(CheckoutRequestDto dto);
        Task Returning(ReturningRequestDto dto);

        Task<bool> UpdateCountingOfProduct(IEnumerable<InvoiceItem> items, ProductCountingState state);

        Task<bool> SendInvoiceToMarketing(Invoice invoice, InvoiceState state);
    }
}
