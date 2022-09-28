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


    }
}
