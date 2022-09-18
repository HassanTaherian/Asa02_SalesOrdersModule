using Domain.Entities;

namespace Domain.Repositories
{
    public interface IInvoiceRepository : IBaseRepository
    {
        IEnumerable<Invoice?> GetInvoices();
        Task<Invoice?> GetInvoiceById(long id);
        Task<Invoice> InsertInvoice(Invoice invoice);
        Invoice UpdateInvoice(Invoice invoice);
        void DeleteInvoice(int id);
    }
}