using Contracts.Discount;
using Contracts.UI;

namespace Services.Abstractions
{
    public interface IDiscountService
    {
        Task<DiscountResponseDto> SendDiscountCodeAsync(DiscountCodeRequestDto discountCodeRequestDto);

        void ApplyDiscountCode(DiscountResponseDto discountResponseDto, long invoiceId);

        Task SetDiscountCodeAsync(DiscountCodeRequestDto discountCodeRequestDto
            , CancellationToken cancellationToken);
    }
}