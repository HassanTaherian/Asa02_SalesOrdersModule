using Contracts.UI;

namespace Services.Abstractions
{
    public interface ISecondCardService
    {
        public Task PutItemInTheSecondCard
        (ProductToSecondCartRequestDto productToSecondCardRequestDto
            , CancellationToken cancellationToken);
    }
        
}
