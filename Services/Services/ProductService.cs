using System;
using System.Collections;
using System.Net.Http.Headers;
using System.Web;
using Contracts.UI.Cart;
using Domain.Entities;
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

        public async Task Add(AddProductRequestDto addProductRequestDto, InvoiceState invoiceState, CancellationToken cancellationToken)
        {
            var item = new InvoiceItem
            {
                ProductId = addProductRequestDto.ProductId,
                Price = addProductRequestDto.UnitPrice,
                Quantity = addProductRequestDto.Quantity
            };

            var invoice = await _invoiceRepository.GetCartOfUser(addProductRequestDto.UserId);

            if (invoice == null)
            {
                var productItem = new Invoice
                {
                    UserId = addProductRequestDto.UserId,
                    InvoiceItems = new List<InvoiceItem>()
                    {
                        item
                    }
                };
                await _invoiceRepository.InsertInvoice(invoice);
            }
            else
            {
                invoice.InvoiceItems.Add(item);
            }

            await _invoiceRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateQuantity(UpdateQuantityRequestDto updateQuantityRequestDto, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetCartOfUser(updateQuantityRequestDto.UserId);
            foreach (var invoiceItem in invoice.InvoiceItems)
            {
                if (invoiceItem.ProductId == updateQuantityRequestDto.ProductId)
                {
                    invoiceItem.Quantity = updateQuantityRequestDto.Quantity;
                }
            }

            await _invoiceRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteItem(DeleteProductRequestDto deleteProductRequestDto)
        {
            var invoice = await _invoiceRepository.GetCartOfUser(deleteProductRequestDto.UserId);
            foreach (var invoiceItem in invoice.InvoiceItems)
            {
                if (invoiceItem.ProductId == deleteProductRequestDto.ProductId)
                {
                    invoiceItem.IsDeleted = true;
                }
            }
        }
    }
}
