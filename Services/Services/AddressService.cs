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
            SetAddressIdAsync(AdditionalInvoiceDataDto additionalInvoiceDataDto,
                CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetCartOfUser
                (additionalInvoiceDataDto.UserId);
            if (invoice != null)
            {
                invoice.AddressId = additionalInvoiceDataDto.AddressId;
                _invoiceRepository.UpdateInvoice(invoice);
                await _invoiceRepository.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
