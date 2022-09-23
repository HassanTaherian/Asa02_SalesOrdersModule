using System.Collections;
using Contracts.UI;

namespace Services.Abstractions
{
    public interface ISecondCartService
    {
        Task<IEnumerable?> GetSecondCartItems
            (ProductToSecondCartResponseDto productToSecondCartResponseDto);

        Task SecondCartToCart
            (ProductToSecondCartRequestDto productToSecondCartRequestDto);

        Task CartToSecondCart
            (ProductToSecondCartRequestDto productToSecondCartRequestDto);

        Task DeleteItemFromTheSecondList
            (ProductToSecondCartRequestDto productToSecondCartRequestDto);
    }
}