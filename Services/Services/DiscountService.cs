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
            SetDiscountCodeAsync(DiscountCodeRequestDto discountCodeRequestDto,
                CancellationToken cancellationToken)
        {
            var invoice = await InvoiceRepository.GetCartOfUser
                (discountCodeRequestDto.UserId);
            if (invoice != null)
            {
                    invoice.DiscountCode = discountCodeRequestDto.DiscountCode;
                    InvoiceRepository.UpdateInvoice(invoice);
                    await InvoiceRepository.SaveChangesAsync(cancellationToken);
            }
        }
    }
}


        
    

