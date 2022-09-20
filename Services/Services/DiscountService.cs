using Contracts.UI;
using Domain.Repositories;
using Services.Abstractions;

namespace Services.Services
{
    public sealed class DiscountService : IDiscountService
    {
        public IInvoiceRepository InvoiceRepository { get; }

        public DiscountService(IInvoiceRepository invoiceRepository) =>
            InvoiceRepository = invoiceRepository;

        public async Task
            SetDiscountCodeAsync(AdditionalInvoiceDataDto additionalInvoiceDataDto,
                CancellationToken cancellationToken)
        {
            var invoice = await InvoiceRepository.GetCartOfUser
                (additionalInvoiceDataDto.UserId);
            if (invoice != null)
            {
                    invoice.AddressId = additionalInvoiceDataDto.AddressId;
                    InvoiceRepository.UpdateInvoice(invoice);
                    await InvoiceRepository.SaveChangesAsync(cancellationToken);
            }
        }

    }
}


        
    

