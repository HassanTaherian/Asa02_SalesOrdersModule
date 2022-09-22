using Contracts.Discount;
using Contracts.UI;

namespace Services.Abstractions
{
    public interface IDiscountService
    {
        Task<DiscountResponseDto> SendDiscountCodeAsync(DiscountCodeRequestDto discountCodeRequestDto);

        Task SetDiscountCodeAsync(DiscountCodeRequestDto discountCodeRequestDto
            , CancellationToken cancellationToken);
    }
}