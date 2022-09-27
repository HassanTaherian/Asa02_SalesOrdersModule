using Contracts.UI;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public InvoiceRepository(RepositoryDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Invoice?> GetInvoices() => _dbContext.Invoices;

        public async Task<Invoice> GetInvoiceById(long id)
        {
            var invoice = await _dbContext.Invoices.Include(invoice => invoice.InvoiceItems)
                .SingleOrDefaultAsync(invoice => invoice.Id == id);

            if (invoice is null)
            {
                throw new InvoiceNotFoundException(id);
            }

            return invoice;
        }

        public async Task<Invoice> GetCartOfUser(int userId)
        {
            var userInvoice = await _dbContext.Invoices
                .Where(invoice => invoice.UserId == userId && invoice.State
                    == InvoiceState.CartState).Include(invoice => invoice.InvoiceItems).FirstOrDefaultAsync();

            if (userInvoice is null)
            {
                throw new CartNotFoundException(userId);
            }

            return userInvoice;
        }

        // Todo: Keep this Remove GetCartOfUser, GetSecondCartOfUser
        public IEnumerable<Invoice?> GetInvoiceByState(int userId, InvoiceState invoiceState)
        {
            var userInvoices = _dbContext.Invoices.Include(invoice => invoice.InvoiceItems)
                .Where(invoice => invoice.UserId == userId &&
                                  invoice.State == invoiceState).ToList();
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

        // Todo: Move to Order Service
        public async Task ChangeInvoiceState(int userId, InvoiceState newState)
        {
            var invoice = await GetCartOfUser(userId);

            invoice.State = newState;
            _dbContext.Entry(invoice).State = EntityState.Modified;
        }

        public async Task<InvoiceItem> GetInvoiceItem(long invoiceId, int productId)
        {
            var invoice = await GetInvoiceById(invoiceId);
            var invoiceItem = invoice.InvoiceItems.SingleOrDefault(invoiceItem => invoiceItem.ProductId == productId);

            if (invoiceItem is null)
            {
                throw new InvoiceItemNotFoundException(invoiceId, productId);
            }

            return invoiceItem;
        }

        public async Task<Invoice?> GetSecondCartOfUser(int userId)
        {
            var userInvoice = await _dbContext.Invoices
                .Where(invoice => invoice.UserId == userId && invoice.State
                    == InvoiceState.SecondCartState)
                .Include(invoice => invoice.InvoiceItems).FirstOrDefaultAsync();

            if (userInvoice is null)
            {
                throw new CartNotFoundException(userId);
            }

            return userInvoice;
        }

        public async Task<IEnumerable<InvoiceItem>> GetItemsOfSecondCart(int userId)
        {
            var invoice = await GetSecondCartOfUser(userId);

            if (invoice == null)
            {
                throw new InvoiceNotFoundException(userId);
            }
            return invoice.InvoiceItems;
        }

        public async Task<InvoiceItem?> GetSecondCartItem(long invoiceId, int productId)
        {
            var invoice = await GetInvoiceById(invoiceId);

            var invoiceItem = (invoice.InvoiceItems).SingleOrDefault
                (item => item.ProductId == productId);

            if (invoiceItem is null)
            {
                throw new InvoiceItemNotFoundException(invoiceId, productId);
            }

            return invoiceItem;
        }
        
        public async Task<IEnumerable<InvoiceItem>> GetNotDeleteItems(long invoiceId)
        {
            var invoice = await GetInvoiceById(invoiceId);

            var invoiceItems = invoice.InvoiceItems.Where(item => item.IsDeleted == false);

            return invoiceItems;
        }

        public async Task<bool> UserHasAnyInvoice(int userId)
        {
            return await _dbContext.Invoices.AnyAsync(invoice =>
                invoice.UserId == userId && invoice.State != InvoiceState.CartState);
        }

        public IList<int> MostFrequentShoppedProducts()
        {
            var products = (
                        from item in _dbContext.InvoiceItems
                        where !item.IsDeleted && !item.IsReturn
                        group item by item.ProductId into productGroup
                        orderby productGroup.Count() descending
                        select productGroup.Key
            ).Take(5).ToList();

            return products;
        }

        public int SaveChanges()
        {
            var returnValue = _dbContext.SaveChanges();
            return returnValue;
        }

        public async Task<int> SaveChangesAsync()

        {
            var returnValue = await _dbContext.SaveChangesAsync();
            return returnValue;
        }
    }
}