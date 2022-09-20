using Contracts.UI;
using Domain.Repositories;
using Services.Abstractions;

namespace Services.Services
{
    public sealed class SecondCardService : ISecondCardService
    {
        public IInvoiceRepository InvoiceRepository { get; }

        public SecondCardService(IInvoiceRepository invoiceRepository)
        {
            InvoiceRepository = invoiceRepository;
        }

        public async Task PutItemInTheSecondCard
        (ProductToSecondCartRequestDto productToSecondCardRequestDto,
            CancellationToken cancellationToken)
        {
            var userCart = await InvoiceRepository.GetInvoiceById
                (productToSecondCardRequestDto.InvoiceId);
            var product = userCart?.InvoiceItems
                        .FirstOrDefault(product => 
                            product.ProductId == productToSecondCardRequestDto.ProductId);
                    if (product != null)
                    {
                        product.IsInSecondCard = true;
                        await InvoiceRepository.SaveChangesAsync(cancellationToken);
                    }
        }
    }
}
