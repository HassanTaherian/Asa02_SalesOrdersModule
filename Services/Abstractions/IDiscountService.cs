using Contracts.UI;

namespace Services.Abstractions
{
    public interface IDiscountService
    {
        public Task SetDiscountCodeAsync(AdditionalInvoiceDataDto additionalInvoiceDataDto
        , CancellationToken cancellationToken);
    }
}
