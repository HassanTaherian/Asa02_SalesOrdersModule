using Contracts.UI.Cart;

namespace Services.Abstractions
{
    public interface IProductService 
    {
        Task Add(AddProductRequestDto addProductRequestDto);
        Task UpdateQuantity(UpdateQuantityRequestDto updateQuantityRequestDto);
        Task DeleteItem();

    }
}
