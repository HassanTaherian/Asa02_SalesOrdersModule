using Contracts.UI.Cart;
using Domain.ValueObjects;

namespace Services.Abstractions
{
    public interface IProductService
    {
        Task AddCart(AddProductRequestDto addProductRequestDto, InvoiceState invoiceState,
            CancellationToken cancellationToken);
        Task UpdateQuantity(UpdateQuantityRequestDto updateQuantityRequestDto, CancellationToken cancellationToken);
        Task DeleteItem(DeleteProductRequestDto deleteProductRequestDto);

    }
}
