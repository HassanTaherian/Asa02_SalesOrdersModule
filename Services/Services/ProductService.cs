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
            if (invoiceItem.Quantity > 0)
            {
                var flag = false;
                foreach (var item in invoice.InvoiceItems)
                {
                    if (item.ProductId != invoiceItem.ProductId) continue;
                    flag = true;
                    item.IsDeleted = false;
                    item.Quantity = invoiceItem.Quantity;
                    _invoiceRepository.UpdateInvoice(invoice);
                }

                if (flag) invoice.InvoiceItems.Add(invoiceItem);
                await _invoiceRepository.SaveChangesAsync();
            }
            else
            {
                throw new QuantityOutOfRangeInputException();
            }
        }

        public async Task UpdateQuantity(UpdateQuantityRequestDto updateQuantityRequestDto)
        {
            var invoice = await _invoiceRepository.GetCartOfUser(updateQuantityRequestDto.UserId);
            var flag = false;

            if (!(updateQuantityRequestDto.Quantity > 0)) throw new QuantityOutOfRangeInputException();

            if (invoice != null)
            {
                foreach (var invoiceItem in invoice.InvoiceItems)
                {
                    if (invoiceItem.ProductId != updateQuantityRequestDto.ProductId) continue;
                    invoiceItem.Quantity = updateQuantityRequestDto.Quantity;
                    invoiceItem.IsDeleted = false;
                    flag = true;
                }
                if (flag)
                    throw new InvoiceItemCartOfUserNotFoundException(updateQuantityRequestDto.UserId,
                        updateQuantityRequestDto.ProductId);
                _invoiceRepository.UpdateInvoice(invoice);
                await _invoiceRepository.SaveChangesAsync();
            }
            else
            {
                throw new CartOfUserNotFoundException(updateQuantityRequestDto.UserId);
            }
        }

        public async Task DeleteItem(DeleteProductRequestDto deleteProductRequestDto)
        {
            var invoice = await _invoiceRepository.GetCartOfUser(deleteProductRequestDto.UserId);
            if (invoice == null) throw new CartOfUserNotFoundException(deleteProductRequestDto.UserId);
            var flag = false;
            foreach (var invoiceItem in invoice.InvoiceItems)
            {
                if (invoiceItem.ProductId != deleteProductRequestDto.ProductId) continue;
                invoiceItem.IsDeleted = true;
                flag = true;
            }

            if (flag == false)
                throw new InvoiceItemCartOfUserNotFoundException(deleteProductRequestDto.UserId,
                    deleteProductRequestDto.ProductId);

            _invoiceRepository.UpdateInvoice(invoice);
            await _invoiceRepository.SaveChangesAsync();
        }
    }
}
