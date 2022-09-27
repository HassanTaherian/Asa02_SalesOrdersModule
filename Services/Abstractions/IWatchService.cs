using Contracts.UI.Watch;

namespace Services.Abstractions
{
    public interface IWatchService
    {
        Task<List<WatchInvoiceItemsResponseDto>> ExistedCartItems(WatchRequestItemsDto watchRequestItemsDto);

        Task<List<WatchInvoiceItemsResponseDto>> IsDeletedCartItems(WatchRequestItemsDto watchRequestItemsDto);

        List<WatchInvoicesResponseDto> OrderInvoices(WatchRequestItemsDto watchRequestItemsDto);

        List<WatchInvoicesResponseDto> ReturnInvoices(WatchRequestItemsDto watchRequestItemsDto);

        Task<List<WatchInvoiceItemsResponseDto>> ShoppedInvoiceItems(WatchInvoicesRequestDto watchInvoicesRequestDto);

        Task<List<WatchInvoiceItemsResponseDto>> ReturnedInvoiceItems(WatchInvoicesRequestDto watchInvoicesRequestDto);

    }
}
