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
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceRepository(RepositoryDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Invoice?> GetInvoices() => _dbContext.Invoices;

        public async Task<Invoice?> GetInvoiceById(long id)
            => await _dbContext.Invoices.FindAsync(id);

        public async Task<Invoice?> GetCartOfUser(int userId)
        {
            var userInvoice = await _dbContext.Invoices
                .Where(invoice => invoice.UserId == userId && invoice.State
                    == InvoiceState.CartState).Include(invoice => invoice.InvoiceItems).FirstOrDefaultAsync();
            return userInvoice;
        }

        public IEnumerable<Invoice?> GetInvoiceByState(int userId, InvoiceState invoiceState)
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

        public int SaveChanges()
        {
            var returnValue = _unitOfWork.SaveChanges();
            return returnValue;
        }

        public async Task<bool> ChangeInvoiceState(int userId, InvoiceState newState)
        {
            var invoice = await GetCartOfUser(userId);

            if (invoice is null)
            {
                // Todo: Throw NotFound Exception
                return false;
            }

            invoice.State = newState;
            _dbContext.Entry(invoice).State = EntityState.Modified;
            return true;
        }

        public async Task<IEnumerable<InvoiceItem>>  GetItemsOfInvoice(int userId)
        {
            var invoice = await GetCartOfUser(userId);
            if (invoice is null)
            {
                // Todo: ItemNotFoundError
                return null;
            }

            return invoice.InvoiceItems;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)

        {
            var returnValue = await _unitOfWork.SaveChangesAsync(cancellationToken);
            return returnValue;
        }
    }
}