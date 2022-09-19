using Contracts.UI;
using Domain.Repositories;
using Domain.ValueObjects;
using Services.Abstractions;

namespace Services.Services
{
    public sealed class DiscountService : IDiscountService
    {
        public IInvoiceRepository InvoiceRepository { get; }

        public DiscountService(IInvoiceRepository invoiceRepository) =>
            InvoiceRepository = invoiceRepository;

        public async Task 
            SetDiscountCodeAsync(AdditionalInvoiceDataDto additionalInvoiceDataDto)
        {
            {
                var invoice = await InvoiceRepository.GetInvoiceByState
                    (additionalInvoiceDataDto.UserId , InvoiceState.CartState);
                if (invoice != null)
                {
                    invoice.DiscountCode = additionalInvoiceDataDto.DiscountCode;
                    InvoiceRepository.UpdateInvoice(invoice);
                    await InvoiceRepository.Save();
                }
            }
        }
    }
}


        
    

