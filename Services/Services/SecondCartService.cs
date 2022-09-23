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
                (productToSecondCartResponseDto.UserId, true);
        }

        public async Task CartToSecondCart
        (ProductToSecondCartRequestDto productToSecondCardRequestDto)
        {
            await InvoiceRepository.FromCartToTheSecondCart
            (productToSecondCardRequestDto.InvoiceId,
                productToSecondCardRequestDto.ProductId);
            await InvoiceRepository.SaveChangesAsync();
        }

        public async Task SecondCartToCart
        (ProductToSecondCartRequestDto productToSecondCardRequestDto)
        {
            await InvoiceRepository.FromSecondCartToTheCart
            (productToSecondCardRequestDto.InvoiceId,
                productToSecondCardRequestDto.ProductId);
            await InvoiceRepository.SaveChangesAsync();
        }

        public async Task DeleteItemFromTheSecondList
        (ProductToSecondCartRequestDto productToSecondCartRequestDto)
        {
            await InvoiceRepository.DeleteItemFromTheSecondCart
            (productToSecondCartRequestDto.InvoiceId,
                productToSecondCartRequestDto.ProductId);
            await InvoiceRepository.SaveChangesAsync();
        }
    }
}