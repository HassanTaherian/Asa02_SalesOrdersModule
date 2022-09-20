using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public  class InvoiceRepository : IInvoiceRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public InvoiceRepository(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Invoice?> GetInvoices() => _dbContext.Invoices;

        public async Task<Invoice?> GetInvoiceById(long id) 
            => await _dbContext.Invoices.FindAsync(id);

        public async Task<Invoice?> GetCartOfUser(int userId)
        {
            var userInvoice =await  _dbContext.Invoices
                .Where(invoice => invoice.UserId == userId && invoice.State
                    == InvoiceState.CartState).FirstOrDefaultAsync();
            return userInvoice;

        }

        public IEnumerable<Invoice> GetInvoiceByState(int userId, InvoiceState invoiceState)
        {
             var userInvoices = _dbContext.Invoices
                 .Where(invoice => invoice.UserId == userId && 
                        invoice.State == invoiceState);
             return userInvoices;
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

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}