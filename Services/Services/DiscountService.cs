using Contracts.UI;
using Domain.Repositories;
using Services.Abstractions;

namespace Services.Services
{
    public sealed class DiscountService : IDiscountService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public DiscountService(IInvoiceRepository invoiceRepository) =>
            _invoiceRepository = invoiceRepository;

        public async Task
            SetDiscountCodeAsync(AdditionalInvoiceDataDto additionalInvoiceDataDto,
                CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetCartOfUser
                (additionalInvoiceDataDto.UserId);
            if (invoice != null)
            {
                    invoice.DiscountCode = additionalInvoiceDataDto.DiscountCode;
                    _invoiceRepository.UpdateInvoice(invoice);
                    await _invoiceRepository.SaveChangesAsync(cancellationToken);
            }
        }

    }
}


        
    

