using Contracts.UI;
using Domain.Repositories;
using Services.Abstractions;

namespace Services.Services
{
    public sealed class AddressService : IAddressService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public AddressService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task 
            SetAddressIdAsync(AddressInvoiceDataDto addressInvoiceDataDto , 
                CancellationToken cancellationToken)
        {
            var invoice =await _invoiceRepository.GetCartOfUser
                (addressInvoiceDataDto.UserId);
            if (invoice != null)
            {
                {
                    invoice.AddressId = addressInvoiceDataDto.AddressId;
                    _invoiceRepository.UpdateInvoice(invoice);
                   await _invoiceRepository.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}
