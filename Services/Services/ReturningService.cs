using Contracts.UI;
using Contracts.UI.Watch;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Exceptions.Returning;
using Domain.Repositories;
using Domain.ValueObjects;
using Services.Abstractions;
using Services.External;

namespace Services.Services;

public class ReturningService : IReturningService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IProductAdapter _productAdapter;
    private readonly IMarketingAdapter _marketingAdapter;

    public ReturningService(IUnitOfWork unitOfWork, IProductAdapter productAdapter, IMarketingAdapter marketingAdapter)
    {
        _unitOfWork = unitOfWork;
        _invoiceRepository = _unitOfWork.InvoiceRepository;
        _productAdapter = productAdapter;
        _marketingAdapter = marketingAdapter;
    }

    public async Task Return(ReturningRequestDto returningRequestDto)
    {
        var invoice = await _invoiceRepository.GetInvoiceById(returningRequestDto.InvoiceId);

        switch (invoice.State)
        {
            case InvoiceState.ReturnState:
                throw new AlreadyReturnedException(returningRequestDto.InvoiceId);
            case InvoiceState.CartState:
                throw new InvoiceIsInCartStateException(returningRequestDto.InvoiceId);
        }

        var invoiceItems = new List<InvoiceItem>();

        foreach (var id in returningRequestDto.ProductIds)
        {
            var invoiceItem = await _invoiceRepository.GetProductOfInvoice(returningRequestDto.InvoiceId, id);

            if (invoiceItem.IsDeleted)
            {
                throw new InvoiceItemNotFoundException(invoice.Id, id);
            }
            invoiceItem.IsReturn = true;
            invoiceItems.Add(invoiceItem);
        }

        await _productAdapter.UpdateCountingOfProduct(invoiceItems, ProductCountingState.ReturnState);
        await _marketingAdapter.SendInvoiceToMarketing(invoice, InvoiceState.ReturnState);

        invoice.ReturnDateTime = DateTime.Now;
        invoice.State = InvoiceState.ReturnState;

        _invoiceRepository.UpdateInvoice(invoice);

        await _unitOfWork.SaveChangesAsync();
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
}