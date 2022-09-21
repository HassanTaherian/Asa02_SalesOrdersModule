using System.Collections;
using Contracts.UI;

namespace Services.Abstractions
{
    public interface ISecondCartService
    {

        Task<IEnumerable?> GetSecondCartItems
            (ProductToSecondCartResponseDto productToSecondCartResponseDto);

        Task ToggleItemInTheCart
        (ProductToSecondCartRequestDto productToSecondCartRequestDto
            , CancellationToken cancellationToken);

        Task DeleteItemFromTheSecondList
        (ProductToSecondCartRequestDto productToSecondCartRequestDto
            , CancellationToken cancellationToken);
    }
        
}
