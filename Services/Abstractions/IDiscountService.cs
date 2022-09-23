using Contracts.Discount;
using Contracts.UI;
using Domain.Entities;

namespace Services.Abstractions
{
    public interface IDiscountService
    {
        Task SetDiscountCodeAsync(DiscountCodeRequestDto discountCodeRequestDto
            , CancellationToken cancellationToken);
    }
}