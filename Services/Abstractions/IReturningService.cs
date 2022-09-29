using Contracts.UI;
using Contracts.UI.Returning;
using Contracts.UI.Watch;

namespace Services.Abstractions;

public interface IReturningService
{
    Task Return(ReturningRequestDto dto);
    List<WatchInvoicesResponseDto> ReturnInvoices(int userId);
    Task<List<WatchInvoiceItemsResponseDto>> ReturnedInvoiceItems(long invoiceId);
}