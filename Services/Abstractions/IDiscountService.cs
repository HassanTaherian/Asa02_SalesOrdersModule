using Contracts.Discount;
using Contracts.UI;
using Domain.Entities;

namespace Services.Abstractions
{
    public interface IDiscountService
    {
        Task<DiscountResponseDto> SendDiscountCodeAsync(DiscountCodeRequestDto discountCodeRequestDto);

        Task<Invoice> ApplyDiscountCode(DiscountResponseDto discountResponseDto, long invoiceId);

        Task SetDiscountCodeAsync(DiscountCodeRequestDto discountCodeRequestDto
            , CancellationToken cancellationToken);
    }
}