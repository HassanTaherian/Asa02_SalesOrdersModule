using Contracts.UI.Cart;
using Contracts.UI.Watch;
using Domain.ValueObjects;

namespace Services.Abstractions
{
    public interface IProductService
    {
        Task AddCart(AddProductRequestDto addProductRequestDto, InvoiceState invoiceState);
        Task UpdateQuantity(UpdateQuantityRequestDto updateQuantityRequestDto);
        Task DeleteItem(DeleteProductRequestDto deleteProductRequestDto);
        
        Task<List<WatchInvoiceItemsResponseDto>> ExistedCartItems(WatchRequestItemsDto watchRequestItemsDto);

        List<WatchInvoiceItemsResponseDto> IsDeletedCartItems(WatchRequestItemsDto watchRequestItemsDto);
    }
}
