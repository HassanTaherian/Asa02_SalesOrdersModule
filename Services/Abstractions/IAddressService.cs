using Contracts.UI;

namespace Services.Abstractions
{
    public interface IAddressService
    {
        public Task SetAddressIdAsync(AdditionalInvoiceDataDto additionalInvoiceDataDto);
    }
}
