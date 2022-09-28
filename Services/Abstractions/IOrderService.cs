using Contracts.UI;
using Contracts.UI.Checkout;
using Contracts.UI.Watch;
using Domain.Entities;
using Domain.ValueObjects;

namespace Services.Abstractions
{
    public interface IOrderService
    {
        Task Checkout(CheckoutRequestDto dto);
        Task Returning(ReturningRequestDto dto);

        List<WatchInvoicesResponseDto> OrderInvoices(WatchRequestItemsDto watchRequestItemsDto);

        List<WatchInvoicesResponseDto> ReturnInvoices(WatchRequestItemsDto watchRequestItemsDto);

        Task<List<WatchInvoiceItemsResponseDto>> ShoppedInvoiceItems(WatchInvoicesRequestDto watchInvoicesRequestDto);

        Task<List<WatchInvoiceItemsResponseDto>> ReturnedInvoiceItems(WatchInvoicesRequestDto watchInvoicesRequestDto);
    }
}
