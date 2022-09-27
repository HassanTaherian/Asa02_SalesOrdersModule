using System.Collections;
using Contracts.UI;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.ValueObjects;
using Services.Abstractions;

namespace Services.Services
{
    public sealed class SecondCartService : ISecondCartService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public SecondCartService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<IEnumerable?> GetSecondCartItems(int userId)
        {
            return await _invoiceRepository.GetItemsOfSecondCart(userId);
        }

        public async Task CartToSecondCart(ProductToSecondCartRequestDto
            productToSecondCardRequestDto)
        {
            var cart = await _invoiceRepository.GetCartOfUser
                (productToSecondCardRequestDto.UserId);

            var cartItem = await _invoiceRepository.GetInvoiceItem
                (productToSecondCardRequestDto.InvoiceId, productToSecondCardRequestDto.ProductId);

            if (cartItem == null)
                throw new InvoiceItemNotFoundException
                    (productToSecondCardRequestDto.InvoiceId, productToSecondCardRequestDto.ProductId);

            var secondCart = await _invoiceRepository.GetSecondCartOfUser(productToSecondCardRequestDto.UserId);
            if (secondCart == null)
            {
                var newSecondCart = new Invoice
                {
                    UserId = productToSecondCardRequestDto.UserId,
                    InvoiceItems = new List<InvoiceItem>(),
                    State = InvoiceState.SecondCartState
                };
                newSecondCart.InvoiceItems.Add(cartItem);
                cart.InvoiceItems.Remove(cartItem);
                await _invoiceRepository.SaveChangesAsync();
            }
            else
            {
                secondCart.InvoiceItems.Add(cartItem);
                cart.InvoiceItems.Remove(cartItem);
                await _invoiceRepository.SaveChangesAsync();
            }
        }

        public async Task SecondCartToCart
            (ProductToSecondCartRequestDto productToSecondCardRequestDto)
        {
            var secondCart = await _invoiceRepository.GetSecondCartOfUser
                (productToSecondCardRequestDto.UserId);

            var secondCartItem = await _invoiceRepository.GetSecondCartItem
                (productToSecondCardRequestDto.InvoiceId,
                productToSecondCardRequestDto.ProductId);

            if (secondCartItem == null)
                throw new InvoiceItemNotFoundException
                    (productToSecondCardRequestDto.InvoiceId, productToSecondCardRequestDto.ProductId);

            var cart = await _invoiceRepository.GetCartOfUser
                (productToSecondCardRequestDto.UserId);

            cart.InvoiceItems.Add(secondCartItem);
            secondCart.InvoiceItems.Remove(secondCartItem);
            await _invoiceRepository.SaveChangesAsync();
        }

        public async Task DeleteItemFromTheSecondCart
            (ProductToSecondCartRequestDto productToSecondCartRequestDto)
        {
            var secondCart = await _invoiceRepository.GetSecondCartOfUser
                (productToSecondCartRequestDto.UserId);

            var secondCartItem = await _invoiceRepository.GetSecondCartItem
            (productToSecondCartRequestDto.InvoiceId, productToSecondCartRequestDto.ProductId);

            if (secondCartItem == null)
                throw new InvoiceItemNotFoundException
                    (productToSecondCartRequestDto.InvoiceId, productToSecondCartRequestDto.ProductId);

            var cart = await _invoiceRepository.
                GetCartOfUser(productToSecondCartRequestDto.UserId);

            cart.InvoiceItems.Add(secondCartItem);

            cart.InvoiceItems.SingleOrDefault(item =>
                item.ProductId == secondCartItem.ProductId)!.IsDeleted = true;

            secondCart.InvoiceItems.Remove(secondCartItem);
            await _invoiceRepository.SaveChangesAsync();
        }
    }
}