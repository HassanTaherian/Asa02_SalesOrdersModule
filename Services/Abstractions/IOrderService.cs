using Contracts.UI.Checkout;
using Contracts.UI.Invoice;
using Contracts.UI.Watch;

namespace Services.Abstractions
{
    public interface IOrderService
    {
        Task Checkout(CheckoutRequestDto dto);

        List<InvoiceResponseDto> GetAllOrdersOfUser(int userId);

        Task<IEnumerable<InvoiceItemResponseDto>> GetInvoiceItemsOfInvoice(long invoiceId);

    }
}
