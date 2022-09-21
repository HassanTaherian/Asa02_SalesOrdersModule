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
            var userCart = await InvoiceRepository.GetCartOfUser
                (productToSecondCartResponseDto.UserId);

            var secondCartItems = userCart?.InvoiceItems
                .Where(item => item.IsInSecondCard);
            return secondCartItems;
        }

        public async Task ToggleItemInTheCart
        (ProductToSecondCartRequestDto productToSecondCardRequestDto,
            CancellationToken cancellationToken)
        {
            var userCart = await InvoiceRepository.GetInvoiceById
                (productToSecondCardRequestDto.InvoiceId);
            var cartItem = userCart?.InvoiceItems
                        .FirstOrDefault(cartItem => 
                            cartItem.ProductId == productToSecondCardRequestDto.ProductId);
            if (cartItem != null)
            {
                cartItem.IsInSecondCard = !(cartItem.IsInSecondCard);
                await InvoiceRepository.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task DeleteItemFromTheSecondList
        (ProductToSecondCartRequestDto productToSecondCartRequestDto,
            CancellationToken cancellationToken)
        {
            var userCart = await InvoiceRepository.GetInvoiceById
                (productToSecondCartRequestDto.InvoiceId);
            var cartItem = userCart?.InvoiceItems
                .FirstOrDefault(cartItem =>
                    cartItem.ProductId == productToSecondCartRequestDto.ProductId);
            if (cartItem != null)
            {
                cartItem.IsDeleted = true;
                await InvoiceRepository.SaveChangesAsync(cancellationToken);
            }
        }

        
    }
}
