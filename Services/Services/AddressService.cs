using Contracts.UI;
using Domain.Exceptions;
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
            SetAddressIdAsync(AddressInvoiceDataDto addressInvoiceDataDto)
        {
            var invoice = await _invoiceRepository.GetCartOfUser
                (addressInvoiceDataDto.UserId);

            if (invoice is null)
            {
                throw new InvoiceNotFoundException(addressInvoiceDataDto.UserId);
            }

            invoice.AddressId = addressInvoiceDataDto.AddressId;
            _invoiceRepository.UpdateInvoice(invoice);
            await _invoiceRepository.SaveChangesAsync();
        }
    }
}