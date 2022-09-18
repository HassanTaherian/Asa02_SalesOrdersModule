using Domain.Entities;

namespace Domain.Repositories
{
    public interface IInvoiceItemRepository
    {
        IEnumerable<InvoiceItem> GetInvoiceItems();
        Task<InvoiceItem?> GetInvoiceItemById(long id);
        Task<InvoiceItem> AddInvoiceItem(InvoiceItem invoiceItem);
        InvoiceItem UpdateInvoiceItem(InvoiceItem invoiceItem);
        void DeleteInvoiceItem(long id);
    }
}
