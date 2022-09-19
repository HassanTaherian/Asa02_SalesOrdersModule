using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Repositories
{
    public interface IInvoiceRepository : IBaseRepository
    {
        IEnumerable<Invoice?> GetInvoices();
        Task<Invoice?> GetInvoiceById(long id);
        Task<Invoice?> GetInvoiceByState(int userId , InvoiceState invoiceState);
        Task<Invoice> InsertInvoice(Invoice invoice);
        Invoice UpdateInvoice(Invoice invoice);
        void DeleteInvoice(int id);
    }
}