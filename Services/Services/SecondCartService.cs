using Contracts.UI;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Exceptions.SecondCart;
using Domain.Repositories;
using Domain.ValueObjects;
using Services.Abstractions;

namespace Services.Services
{
    public sealed class SecondCartService : ISecondCartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInvoiceRepository _invoiceRepository;

        public SecondCartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _invoiceRepository = unitOfWork.InvoiceRepository;
        }

        public IEnumerable<InvoiceItem> GetSecondCartItems(int userId)
        {
            return _invoiceRepository.GetSecondCartOfUser(userId).InvoiceItems;
        }

        public async Task CartToSecondCart(ProductToSecondCartRequestDto productToSecondCardRequestDto)
        {
            var cart = _invoiceRepository.GetCartOfUser(productToSecondCardRequestDto.UserId);
            var cartItem = await _invoiceRepository.GetProductOfInvoice(productToSecondCardRequestDto.InvoiceId, productToSecondCardRequestDto.ProductId);

            Invoice secondCart;
            try
            {
                secondCart = _invoiceRepository.GetSecondCartOfUser(productToSecondCardRequestDto.UserId);
            }
            catch(SecondCartNotFoundException)
            {
                secondCart = new Invoice
                {
                    UserId = productToSecondCardRequestDto.UserId,
                    InvoiceItems = new List<InvoiceItem>(),
                    State = InvoiceState.SecondCartState
                };
            }
            
            secondCart.InvoiceItems.Add(cartItem);
            cart.InvoiceItems.Remove(cartItem);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SecondCartToCart(ProductToSecondCartRequestDto productToSecondCardRequestDto)
        {
            var secondCart = _invoiceRepository.GetSecondCartOfUser(productToSecondCardRequestDto.UserId);

            var secondCartItem = await _invoiceRepository.GetProductOfInvoice
            (productToSecondCardRequestDto.InvoiceId,
                productToSecondCardRequestDto.ProductId);

            if (secondCartItem == null)
                throw new InvoiceItemNotFoundException
                    (productToSecondCardRequestDto.InvoiceId, productToSecondCardRequestDto.ProductId);

            var cart = _invoiceRepository.GetCartOfUser
                (productToSecondCardRequestDto.UserId);

            cart.InvoiceItems.Add(secondCartItem);
            secondCart.InvoiceItems.Remove(secondCartItem);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteItemFromTheSecondCart
            (ProductToSecondCartRequestDto productToSecondCartRequestDto)
        {
            var secondCart = _invoiceRepository.GetSecondCartOfUser
                (productToSecondCartRequestDto.UserId);

            var secondCartItem = await _invoiceRepository.GetProductOfInvoice
                (productToSecondCartRequestDto.InvoiceId, productToSecondCartRequestDto.ProductId);

            if (secondCartItem == null)
                throw new InvoiceItemNotFoundException
                    (productToSecondCartRequestDto.InvoiceId, productToSecondCartRequestDto.ProductId);

            var cart = _invoiceRepository.GetCartOfUser(productToSecondCartRequestDto.UserId);

            cart.InvoiceItems.Add(secondCartItem);

            cart.InvoiceItems.SingleOrDefault(item =>
                item.ProductId == secondCartItem.ProductId)!.IsDeleted = true;

            secondCart.InvoiceItems.Remove(secondCartItem);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}