using Contracts.UI;

namespace Services.Abstractions
{
    public interface IDiscountService
    {
        public Task SetDiscountCodeAsync(DiscountCodeRequestDto discountCodeRequestDto
            , CancellationToken cancellationToken);
    }
}
