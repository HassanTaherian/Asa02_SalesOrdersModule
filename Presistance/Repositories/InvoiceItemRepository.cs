using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class InvoiceItemRepository : IInvoiceItemRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public InvoiceItemRepository(RepositoryDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IEnumerable<InvoiceItem> GetInvoiceItems()
        {
            return _dbContext.InvoiceItems;
        }

        public async Task<InvoiceItem?> GetInvoiceItemById(long id)
        {
            var result = await _dbContext.InvoiceItems.FindAsync(id);
            return result;
        }

        public async Task<InvoiceItem> AddInvoiceItem(InvoiceItem invoiceItem)
        {
            await _dbContext.InvoiceItems.AddAsync(invoiceItem);
            return invoiceItem;
        }

        public InvoiceItem UpdateInvoiceItem(InvoiceItem invoiceItem)
        {
            _dbContext.InvoiceItems.Attach(invoiceItem);
            _dbContext.Entry(invoiceItem).State = EntityState.Modified;

            return invoiceItem;
        }

        public async void DeleteInvoiceItem(long id)
        {
            var invoiceItem = await GetInvoiceItemById(id);
            if (invoiceItem is null)
            {
                return;
            }

            _dbContext.InvoiceItems.Remove(invoiceItem);
        }
    }
}
