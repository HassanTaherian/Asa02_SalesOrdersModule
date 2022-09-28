using Contracts.UI.Cart;
using Contracts.UI.Watch;
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

            try
            {
                var invoice = _invoiceRepository.GetCartOfUser(addProductRequestDto.UserId);
                await AddItemToInvoice(invoice, item);
            }
            catch (CartNotFoundException)
            {
                await AddItemToNewInvoice(addProductRequestDto.UserId, item);
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
            if (invoiceItem.Quantity <= 0)
            {
                throw new QuantityOutOfRangeInputException();
            }

            var newInvoice = new Invoice
            {
                UserId = userId,
                InvoiceItems = new List<InvoiceItem> { invoiceItem }
            };
            await _invoiceRepository.InsertInvoice(newInvoice);
            await _invoiceRepository.SaveChangesAsync();
        }

        private async Task AddItemToInvoice(Invoice invoice, InvoiceItem invoiceItem)
        {
            if (invoiceItem.Quantity <= 0)
            {
                throw new QuantityOutOfRangeInputException();
            }


            try
            {
                var existedItem = await _invoiceRepository.GetProductOfInvoice(invoice.Id, invoiceItem.ProductId);
                existedItem.IsDeleted = false;
                existedItem.Quantity = invoiceItem.Quantity;
                existedItem.Price = invoiceItem.Price;
            }
            catch (InvoiceItemNotFoundException)
            {
                invoice.InvoiceItems.Add(invoiceItem);
            }

            _invoiceRepository.UpdateInvoice(invoice);


            await _invoiceRepository.SaveChangesAsync();
        }

        public async Task UpdateQuantity(UpdateQuantityRequestDto updateQuantityRequestDto)
        {
            var cart = _invoiceRepository.GetCartOfUser(updateQuantityRequestDto.UserId);

            if (updateQuantityRequestDto.Quantity <= 0)
            {
                throw new QuantityOutOfRangeInputException();
            }


            var existed = await _invoiceRepository.GetProductOfInvoice(cart.Id, updateQuantityRequestDto.ProductId);

            existed.Quantity = updateQuantityRequestDto.Quantity;
            existed.IsDeleted = false;

            _invoiceRepository.UpdateInvoice(cart);
            await _invoiceRepository.SaveChangesAsync();
        }

        public async Task DeleteItem(DeleteProductRequestDto deleteProductRequestDto)
        {
            var cart = _invoiceRepository.GetCartOfUser(deleteProductRequestDto.UserId);

            var existedItem = await _invoiceRepository.GetProductOfInvoice(cart.UserId, deleteProductRequestDto.ProductId);

            existedItem.IsDeleted = true;
            _invoiceRepository.UpdateInvoice(cart);
            await _invoiceRepository.SaveChangesAsync();
        }
        
        public async Task<List<WatchInvoiceItemsResponseDto>> ExistedCartItems(WatchRequestItemsDto watchRequestItemsDto)
        {
            // var invoiceItems = await _invoiceRepository.GetExistedItemsOfCart(watchRequestItemsDto.UserId, false, false);
            // if (invoiceItems == null || !invoiceItems.Any())
            // {
            //     throw new EmptyCartException(watchRequestItemsDto.UserId);
            // }
            //
            // return MapWatchCartItemDto(invoiceItems);
            return null;
        }

        private List<WatchInvoiceItemsResponseDto> MapWatchCartItemDto(IEnumerable<InvoiceItem> invoiceItems)
        {
            return invoiceItems.Select(invoiceItem => new WatchInvoiceItemsResponseDto
                {
                    ProductId = invoiceItem.ProductId,
                    Quantity = invoiceItem.Quantity,
                    UnitPrice = invoiceItem.Price
                })
                .ToList();
        }

        public List<WatchInvoiceItemsResponseDto> IsDeletedCartItems(WatchRequestItemsDto watchRequestItemsDto)
        {
            var invoice = _invoiceRepository.GetCartOfUser(watchRequestItemsDto.UserId);
            var invoiceItems = invoice.InvoiceItems;
            if (invoiceItems == null || !invoiceItems.Any())
            {
                throw new EmptyCartException(watchRequestItemsDto.UserId);
            }

            return MapWatchCartItemDto(invoiceItems);

        }
    }
}