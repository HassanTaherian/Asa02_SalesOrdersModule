using System.Collections;
using Contracts.UI;

namespace Services.Abstractions
{
    public interface ISecondCartService
    {

        Task<IEnumerable?> GetSecondCartItems
            (ProductToSecondCartResponseDto productToSecondCartResponseDto);

        Task PutItemInTheSecondCard
        (ProductToSecondCartRequestDto productToSecondCartRequestDto
            , CancellationToken cancellationToken);

        Task BackItemToTheCart
        (ProductToSecondCartRequestDto productToSecondCartRequestDto
            , CancellationToken cancellationToken);

        Task DeleteItemFromTheSecondList
        (ProductToSecondCartRequestDto productToSecondCartRequestDto
            , CancellationToken cancellationToken);
    }
        
}
