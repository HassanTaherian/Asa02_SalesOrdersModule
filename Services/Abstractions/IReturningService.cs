using Contracts.UI;
using Contracts.UI.Invoice;
using Contracts.UI.Returning;
using Contracts.UI.Watch;

namespace Services.Abstractions;

public interface IReturningService
{
    Task Return(ReturningRequestDto dto);
    List<InvoiceResponseDto> ReturnInvoices(int userId);
    Task<List<WatchInvoiceItemsResponseDto>> ReturnedInvoiceItems(long invoiceId);
}