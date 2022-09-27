using Contracts.UI.Watch;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.ValueObjects;
using Services.Abstractions;

namespace Services.Services
{
    public class WatchService : IWatchService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public WatchService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
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

        public async Task<List<WatchInvoiceItemsResponseDto>> IsDeletedCartItems(WatchRequestItemsDto watchRequestItemsDto)
        {
            var invoice = await _invoiceRepository.GetCartOfUser(watchRequestItemsDto.UserId);
            var invoiceItems = invoice.InvoiceItems;
            if (invoiceItems == null || !invoiceItems.Any())
            {
                throw new EmptyCartException(watchRequestItemsDto.UserId);
            }

            return MapWatchCartItemDto(invoiceItems);

        }

        public List<WatchInvoicesResponseDto> OrderInvoices(WatchRequestItemsDto watchRequestItemsDto)
        {
            var invoices = _invoiceRepository.GetInvoiceByState(watchRequestItemsDto.UserId, InvoiceState.OrderState);
            if (invoices == null)
            {
                throw new InvoiceNotFoundException(watchRequestItemsDto.UserId);
            }

            return MapWatchOrderDto(invoices);

        }

        private static List<WatchInvoicesResponseDto> MapWatchOrderDto(IEnumerable<Invoice> invoices)
        {
            return invoices.Select(invoice => new WatchInvoicesResponseDto
            {
                InvoiceId = invoice.Id,
                DateTime = (DateTime)invoice.ShoppingDateTime
            })
                .ToList();
        }

        public List<WatchInvoicesResponseDto> ReturnInvoices(WatchRequestItemsDto watchRequestItemsDto)
        {
            var invoices = _invoiceRepository.GetInvoiceByState(watchRequestItemsDto.UserId, InvoiceState.ReturnState);
            if (invoices == null)
            {
                throw new InvoiceNotFoundException(watchRequestItemsDto.UserId);
            }

            return MapWatchReturnDto(invoices);

        }

        private static List<WatchInvoicesResponseDto> MapWatchReturnDto(IEnumerable<Invoice> invoices)
        {
            return invoices.Select(invoice => new WatchInvoicesResponseDto
            {
                InvoiceId = invoice.Id,
                DateTime = (((DateTime)invoice.ReturnDateTime)),
            })
                .ToList();
        }

        public async Task<List<WatchInvoiceItemsResponseDto>> ShoppedInvoiceItems(WatchInvoicesRequestDto watchInvoicesRequestDto)
        {
            var invoiceItems = await _invoiceRepository.GetNotDeleteItems(watchInvoicesRequestDto.InvoiceId);
            if (invoiceItems == null)
            {
                throw new EmptyInvoiceException(watchInvoicesRequestDto.InvoiceId);
            }

            return MapWatchCartItemDto(invoiceItems);

        }

        public async Task<List<WatchInvoiceItemsResponseDto>> ReturnedInvoiceItems(WatchInvoicesRequestDto watchInvoicesRequestDto)
        {
            var invoice = await _invoiceRepository.GetInvoiceById(watchInvoicesRequestDto.InvoiceId);

            var invoiceItems = 
                (from invoiceItem in invoice.InvoiceItems 
                    where invoiceItem.IsReturn == true 
                    select new WatchInvoiceItemsResponseDto 
                { ProductId = invoiceItem.ProductId,
                    Quantity = invoiceItem.Quantity,
                    UnitPrice = invoiceItem.Price })
                .ToList();

            if (invoiceItems == null)
            {
                throw new EmptyInvoiceException(watchInvoicesRequestDto.InvoiceId);
            }

            return invoiceItems;
        }


    }
}
