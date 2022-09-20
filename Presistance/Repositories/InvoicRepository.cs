using Contracts.UI.Cart;
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

        public IEnumerable<Invoice?> GetInvoices() => _dbContext.Invoices;
        IEnumerable<Invoice> IInvoiceRepository.GetInvoiceByState(int userId, InvoiceState invoiceState)
        {
            throw new NotImplementedException();
        }

        public async Task<Invoice?> GetInvoiceById(long id)
            => await _dbContext.Invoices.FindAsync(id);

        public Task<Invoice?> GetCartOfUser(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Invoice?> GetInvoiceByState(int userId, InvoiceState invoiceState)
        {
            var userInvoice = await _dbContext.Invoices
                .Where(invoice => invoice.UserId == userId &&
                                  invoice.State == invoiceState).FirstOrDefaultAsync();
            return userInvoice;
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