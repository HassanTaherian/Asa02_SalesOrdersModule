using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public InvoiceRepository(RepositoryDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<Invoice?>> GetInvoices()
        {
            return await _dbContext.Invoices.ToListAsync();
        }

        public async Task<Invoice?> GetInvoiceById(int id)
        {
            return await _dbContext.Invoices.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Invoice?> AddInvoice(Invoice invoice)
        {
            var result = await _dbContext.Invoices.AddAsync(invoice);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Invoice?> UpdateInvoice(Invoice invoice)
        {
            var result = await _dbContext.Invoices.FirstOrDefaultAsync
                (e => e != null && e.Id == invoice.Id);
            if (result == null) return null;

            result.UserId = invoice.UserId;
            result.TotalPrice = invoice.TotalPrice;
            result.Address = invoice.Address;
            result.NewTotalPrice = invoice.NewTotalPrice;
            result.InvoiceItems = invoice.InvoiceItems;
            result.DifferencePrice = invoice.DifferencePrice;
            result.DiscountCode = invoice.DiscountCode;
            result.ShoppingDateTime = invoice.ShoppingDateTime;
            result.State = invoice.State;

            await _dbContext.SaveChangesAsync();
            return result;
        }

        public async Task DeleteInvoice(int id)
        {
            var result = await _dbContext.Invoices
                .FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _dbContext.Invoices.Remove(result);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
