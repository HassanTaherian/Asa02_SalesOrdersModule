using Domain.Entities;
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

        public async Task<IEnumerable<InvoiceItem?>> GetInvoiceItems()
        {
            return await _dbContext.InvoiceItems.ToListAsync();
        }

        public async Task<InvoiceItem?> GetInvoiceItemById(int id)
        {
            return await _dbContext.InvoiceItems.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<InvoiceItem?> AddInvoiceItem(InvoiceItem invoiceItem)
        {
            var result = await _dbContext.InvoiceItems.AddAsync(invoiceItem);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<InvoiceItem?> UpdateInvoiceItem(InvoiceItem invoiceItem)
        {
            var result = await _dbContext.InvoiceItems.FirstOrDefaultAsync
                (e => e.Id == invoiceItem.Id);
            if (result == null) return null;

            result.ProductId = invoiceItem.ProductId;
            result.Price = invoiceItem.Price;
            result.IsReturn = invoiceItem.IsReturn;
            result.Quantity = invoiceItem.Quantity;
            result.ReturnDateTime = invoiceItem.ReturnDateTime;

            await _dbContext.SaveChangesAsync();
            return result;
        }

        public async Task DeleteInvoiceItem(int id)
        {
            var result = await _dbContext.InvoiceItems
                .FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _dbContext.InvoiceItems.Remove(result);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
