using Contracts.UI;

namespace Services.Abstractions
{
    public interface IAddressService
    {
         Task SetAddressIdAsync(AddressInvoiceDataDto additionalInvoiceDataDto ,
             CancellationToken cancellationToken);
    }
}
