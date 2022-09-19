using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public InvoiceRepository(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Invoice?> GetInvoices()
        {
            return _dbContext.Invoices;
        }

        public async Task<Invoice?> GetInvoiceById(long id)
        {
            return await _dbContext.Invoices.FindAsync(id);
        }
        
        public async Task<Invoice?> GetInvoiceByUserId(int userId)
        {
            var invoice = await _dbContext.Invoices.
                SingleOrDefaultAsync(invoice => invoice.UserId == userId);

            return invoice;
        }

        public async Task<Invoice> InsertInvoice(Invoice invoice)
        {
            await _dbContext.Invoices.AddAsync(invoice);
            return invoice;
        }

        public Invoice UpdateInvoice(Invoice invoice)
        {
            _dbContext.Invoices.Attach(invoice);
            _dbContext.Entry(invoice).State = EntityState.Modified;


            return invoice;
        }

        public async void DeleteInvoice(int id)
        {
            var invoice = await GetInvoiceById(id);

            if (invoice is null)
            {
                return;
            }

            _dbContext.Invoices.Remove(invoice);
        }

        public async Task<bool> ChangeInvoiceState(int userId, InvoiceState newState)
        {
            var invoice = await GetInvoiceByUserId(userId);

            if (invoice is null)
            {
                // Todo: Throw NotFound Exception
                return false;
            }
            invoice.State = newState;
            _dbContext.Entry(invoice).State = EntityState.Modified;
            return true;
        }

        public async Task<IEnumerable<InvoiceItem>> GetItemsOfInvoice(int userId)
        {
            var invoice = await GetInvoiceByUserId(userId);
            if (invoice is null)
            {
                // Todo: ItemNotFoundError
                return null;
            }

            return invoice.InvoiceItems;
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}