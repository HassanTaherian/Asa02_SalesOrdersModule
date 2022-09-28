using Contracts.UI.Checkout;
using Contracts.UI.Watch;

namespace Services.Abstractions
{
    public interface IOrderService
    {
        Task Checkout(CheckoutRequestDto dto);

        List<WatchInvoicesResponseDto> OrderInvoices(WatchRequestItemsDto watchRequestItemsDto);
        
        Task<List<WatchInvoiceItemsResponseDto>> ShoppedInvoiceItems(WatchInvoicesRequestDto watchInvoicesRequestDto);

    }
}
