using Contracts.UI;

namespace Services.Abstractions
{
    public interface IAddressService
    {
         Task SetAddressIdAsync(AdditionalInvoiceDataDto additionalInvoiceDataDto ,
             CancellationToken cancellationToken);
    }
}
