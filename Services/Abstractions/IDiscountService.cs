using Contracts.UI;

namespace Services.Abstractions
{
    public interface IDiscountService
    {
        Task SetDiscountCodeAsync(DiscountCodeRequestDto discountCodeRequestDto);
    }
}