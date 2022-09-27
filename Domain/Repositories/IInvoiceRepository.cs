using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Repositories
{
    public interface IInvoiceRepository : IUnitOfWork
    {
        IEnumerable<Invoice?> GetInvoices();

        IEnumerable<Invoice?> GetInvoiceByState(int userId, InvoiceState invoiceState);

        Task<Invoice> GetInvoiceById(long id);

        Task<Invoice> GetCartOfUser(int userId);

        Task<Invoice> InsertInvoice(Invoice invoice);

        Invoice UpdateInvoice(Invoice invoice);

        Task ChangeInvoiceState(int userId, InvoiceState newState);

        Task<InvoiceItem> GetInvoiceItem(long invoiceId, int productId);

        // Task<IEnumerable<InvoiceItem>> GetItemsOfInvoice(int userId);

        Task<Invoice?> GetSecondCartOfUser(int userId);

        Task<IEnumerable<InvoiceItem>> GetItemsOfSecondCart(int userId);

        Task<InvoiceItem?> GetSecondCartItem(long invoiceId, int productId);

        Task<IEnumerable<InvoiceItem>> GetNotDeleteItems(long invoiceId);

        //  Task GetProductItemById(int productId);
        Task<bool> UserHasAnyInvoice(int userId);

        IList<int> MostFrequentShoppedProducts();
    }
}