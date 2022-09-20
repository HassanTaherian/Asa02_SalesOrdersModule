using System.Collections;
using Contracts.UI;

namespace Services.Abstractions
{
    public interface ISecondCartService
    {
        Task PutItemInTheSecondCard
        (ProductToSecondCartRequestDto productToSecondCardRequestDto
            , CancellationToken cancellationToken);

        Task<IEnumerable?> GetSecondCartItems
            (ProductToSecondCartResponseDto productToSecondCartResponseDto);
    }
        
}
