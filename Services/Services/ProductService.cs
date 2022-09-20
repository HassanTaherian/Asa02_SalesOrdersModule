using System;
using System.Collections;
using System.Net.Http.Headers;
using System.Web;
using Contracts.UI.Cart;
using Domain.Entities;
using Domain.Repositories;
using Services.Abstractions;

namespace Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceItemRepository _invoiceItemRepository;

        public ProductService(IInvoiceRepository invoiceRepository,
            IInvoiceItemRepository invoiceItemRepository)
        {
            _invoiceRepository = invoiceRepository;
            _invoiceItemRepository = invoiceItemRepository;
        }

        public async Task Add(AddProductRequestDto addProductRequestDto)
        {
            var product = new InvoiceItem
            {
                ProductId = addProductRequestDto.ProductId,
                Price = addProductRequestDto.UnitPrice,
                Quantity = addProductRequestDto.Quantity
            };
            //do these two lines with repo
            ICollection<InvoiceItem> productItems = new List<InvoiceItem>();
            productItems.Add(product);
            //--------
            var productItem = new Invoice
            {
                UserId = addProductRequestDto.UserId,
                InvoiceItems = productItems

            };
            await _invoiceRepository.InsertInvoice(productItem);
            await _invoiceRepository.Save();
        }

        public async Task UpdateQuantity(UpdateQuantityRequestDto updateQuantityRequestDto)
        {
            //get PreOrder invoice with user id (Ali did it) it should return invoice
            // var invoice = Search with user id;
            // invoice.

        }

        public async Task DeleteItem()
        {
            //get PreOrder invoice with user id (Ali did it) it should return invoice
            // var invoice = Search with user id;
            // invoice.
        }


    }
}
