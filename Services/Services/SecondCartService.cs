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

        public async Task<IEnumerable?> GetSecondCartItems
            (ProductToSecondCartResponseDto productToSecondCartResponseDto)
        {
            return await InvoiceRepository.GetItemsOfCart
                (productToSecondCartResponseDto.UserId , true);
        }

        public async Task ToggleItemInTheCart
        (ProductToSecondCartRequestDto productToSecondCardRequestDto , 
            CancellationToken cancellationToken)
        {
            await InvoiceRepository.ToggleItemInTheCart
            (productToSecondCardRequestDto.InvoiceId,
                productToSecondCardRequestDto.ProductId);
            await InvoiceRepository.SaveChangesAsync();
        }

        public async Task DeleteItemFromTheSecondList
        (ProductToSecondCartRequestDto productToSecondCartRequestDto,
            CancellationToken cancellationToken)
        {
            await InvoiceRepository.DeleteItemFromTheSecondCart
            (productToSecondCartRequestDto.InvoiceId,
                productToSecondCartRequestDto.ProductId);
            await InvoiceRepository.SaveChangesAsync();
        }

        
    }
}
