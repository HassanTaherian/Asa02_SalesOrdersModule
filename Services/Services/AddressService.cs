using Contracts.UI;
using Domain.Repositories;
using Domain.ValueObjects;
using Services.Abstractions;

namespace Services.Services
{
    public sealed class AddressService : IAddressService
    {
        public IInvoiceRepository InvoiceRepository { get; }

        public AddressService(IInvoiceRepository invoiceRepository) =>
            InvoiceRepository = invoiceRepository;

        public async Task 
            SetAddressIdAsync(AdditionalInvoiceDataDto additionalInvoiceDataDto)
        {
            var invoice = await InvoiceRepository.GetInvoiceByState
                (additionalInvoiceDataDto.UserId , InvoiceState.CartState);
                    if (invoice != null)
                    {
                        invoice.AddressId = additionalInvoiceDataDto.AddressId;
                        InvoiceRepository.UpdateInvoice(invoice);
                        await InvoiceRepository.Save();
                    }
        }
    }
}
