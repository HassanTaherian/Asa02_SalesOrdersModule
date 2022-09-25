using Contracts.UI.Cart;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.ValueObjects;
using Services.Abstractions;

namespace Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public ProductService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task AddCart(AddProductRequestDto addProductRequestDto, InvoiceState invoiceState)
        {
            var item = MapDtoToInvoiceItem(addProductRequestDto);

            var invoice = await _invoiceRepository.GetCartOfUser(addProductRequestDto.UserId);

            if (invoice == null)
            {
                await AddItemToNewInvoice(addProductRequestDto.UserId, item);
            }
            else
            {
                // Todo: Not Working
                await AddItemToInvoice(invoice, item);
            }
        }

        private static InvoiceItem MapDtoToInvoiceItem(AddProductRequestDto addProductRequestDto)
        {
            var item = new InvoiceItem
            {
                ProductId = addProductRequestDto.ProductId,
                Price = addProductRequestDto.UnitPrice,
                Quantity = addProductRequestDto.Quantity
            };
            return item;
        }

        private async Task AddItemToNewInvoice(int userId, InvoiceItem invoiceItem)
        {
            if (invoiceItem.Quantity > 0)
            {
                var newInvoice = new Invoice
                {
                    UserId = userId,
                    InvoiceItems = new List<InvoiceItem> { invoiceItem }
                };
                await _invoiceRepository.InsertInvoice(newInvoice);
                await _invoiceRepository.SaveChangesAsync();
            }
            else
            {
                throw new QuantityOutOfRangeInputException();
            }
        }

        private async Task AddItemToInvoice(Invoice invoice, InvoiceItem invoiceItem)
        {
            if (invoiceItem.Quantity <= 0)
            {
                throw new QuantityOutOfRangeInputException();
            }

            var existedItem = await _invoiceRepository.GetInvoiceItem(invoice.Id, invoiceItem.ProductId);

            if (existedItem is null)
            {
                invoice.InvoiceItems.Add(invoiceItem);
            }
            else
            {
                existedItem.IsDeleted = false;
                existedItem.Quantity = invoiceItem.Quantity;
                existedItem.Price = invoiceItem.Price;
                _invoiceRepository.UpdateInvoice(invoice);
            }


            await _invoiceRepository.SaveChangesAsync();
        }

        public async Task UpdateQuantity(UpdateQuantityRequestDto updateQuantityRequestDto)
        {
            var cart = await _invoiceRepository.GetCartOfUser(updateQuantityRequestDto.UserId);

            if (!(updateQuantityRequestDto.Quantity > 0))
            {
                throw new QuantityOutOfRangeInputException();
            }

            if (cart is null)
            {
                throw new CartOfUserNotFoundException(updateQuantityRequestDto.UserId);
            }

            var existed = await _invoiceRepository.GetInvoiceItem(cart.Id, updateQuantityRequestDto.ProductId);

            if (existed is null)
            {
                throw new InvoiceItemCartOfUserNotFoundException(updateQuantityRequestDto.UserId,
                    updateQuantityRequestDto.ProductId);
            }

            existed.Quantity = updateQuantityRequestDto.Quantity;
            existed.IsDeleted = false;

            _invoiceRepository.UpdateInvoice(cart);
            await _invoiceRepository.SaveChangesAsync();
        }

        public async Task DeleteItem(DeleteProductRequestDto deleteProductRequestDto)
        {
            var cart = await _invoiceRepository.GetCartOfUser(deleteProductRequestDto.UserId);


            if (cart is null)
            {
                throw new CartOfUserNotFoundException(deleteProductRequestDto.UserId);
            }

            var existedItem = await _invoiceRepository.GetInvoiceItem(cart.UserId, deleteProductRequestDto.ProductId);

            if (existedItem is null)
            {
                throw new InvoiceItemCartOfUserNotFoundException(deleteProductRequestDto.UserId,
                    deleteProductRequestDto.ProductId);
            }

            existedItem.IsDeleted = true;
            _invoiceRepository.UpdateInvoice(cart);
            await _invoiceRepository.SaveChangesAsync();
        }
    }
}