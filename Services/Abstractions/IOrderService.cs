using Contracts.UI.Checkout;
using Contracts.UI.Watch;

namespace Services.Abstractions
{
    public interface IOrderService
    {
        Task Checkout(CheckoutRequestDto dto);

        List<WatchInvoicesResponseDto> GetAllOrdersOfUser(int userId);
        
        Task<List<WatchInvoiceItemsResponseDto>> GetInvoiceItemsOfInvoice(long invoiceId);

    }
}
