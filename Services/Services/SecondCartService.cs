using System.Collections;
using Contracts.UI;
using Domain.Repositories;
using Services.Abstractions;

namespace Services.Services
{
    public sealed class SecondCartService : ISecondCartService
    {
        public IInvoiceRepository InvoiceRepository { get; }

        public SecondCartService(IInvoiceRepository invoiceRepository)
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

        public async Task<IEnumerable?> GetSecondCartItems
        (ProductToSecondCartResponseDto productToSecondCartResponseDto)
        {
            var userCart = await InvoiceRepository.GetCartOfUser
                (productToSecondCartResponseDto.UserId);

            var secondCartItems = userCart?.InvoiceItems
                    .Where(item => item.IsInSecondCard);
                return secondCartItems;
        }
    }
}
