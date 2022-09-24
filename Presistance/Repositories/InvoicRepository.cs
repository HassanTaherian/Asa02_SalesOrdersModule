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
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceRepository(RepositoryDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Invoice?> GetInvoices() => _dbContext.Invoices;

        public async Task<Invoice> GetInvoiceById(long id)
        {
            var invoice = await _dbContext.Invoices.Include(invoice => invoice.InvoiceItems)
                .SingleAsync(invoice => invoice.Id == id);

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

        public IEnumerable<Invoice?> GetInvoiceByState(int userId, InvoiceState invoiceState)
        {
            var userInvoices = _dbContext.Invoices.Include(invoice => invoice.InvoiceItems)
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

        public async Task<InvoiceItem> GetInvoiceItem(long invoiceId, int productId)
        {
            var invoice = await GetInvoiceById(invoiceId);
            var invoiceItem = invoice.InvoiceItems.Single(invoiceItem => invoiceItem.ProductId == productId);

            if (invoiceItem is null)
            {
                throw new InvoiceItemNotFoundException(invoiceId, productId);
            }

            return invoiceItem;
        }

        public async Task<IEnumerable<InvoiceItem>> GetItemsOfInvoice(int userId)
        {
            var invoice = await GetCartOfUser(userId);
            if (invoice is null)
            {
                // Todo: ItemNotFoundError
                return null;
            }

            return invoice.InvoiceItems.Where
                (invoiceItem => invoiceItem.IsInSecondCard = false);
        }


        public async Task<IEnumerable<InvoiceItem>?> GetItemsOfCart(int userId, bool isInSecondCart)
        {
            var invoice = await GetCartOfUser(userId);
            return invoice.InvoiceItems.Where
                (invoiceItem => invoiceItem.IsInSecondCard = isInSecondCart);
        }

        public async Task<IEnumerable<InvoiceItem>> GetNotDeleteItems(long invoiceId)
        {
            var invoice = await GetInvoiceById(invoiceId);

            var invoiceItems = invoice.InvoiceItems.Where(item => item.IsDeleted == false);

            return invoiceItems;
        }

        public async Task FromCartToTheSecondCart(long invoiceId, int productId)
        {
            var invoice = await GetInvoiceById(invoiceId);

            var cartItem = (invoice.InvoiceItems).
                FirstOrDefault(item => item.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.IsInSecondCard = true;
            }
        }

        public async Task FromSecondCartToTheCart(long invoiceId, int productId)
        {
            var invoice = await GetInvoiceById(invoiceId);
            var cartItem = invoice?.InvoiceItems
                .FirstOrDefault(item =>
                    item.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.IsInSecondCard = false;
            }
        }

        public async Task DeleteItemFromTheSecondCart(long invoiceId, int productId)
        {
            var invoice = await GetInvoiceById(invoiceId);

            var cartItem = invoice?.InvoiceItems
                .FirstOrDefault(cartItem =>
                    cartItem.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.IsDeleted = true;
            }
        }

        public int SaveChanges()
        {
            var returnValue = _unitOfWork.SaveChanges();
            return returnValue;
        }

        public async Task<int> SaveChangesAsync()

        {
            var returnValue = await _dbContext.SaveChangesAsync();
            return returnValue;
        }
    }
}