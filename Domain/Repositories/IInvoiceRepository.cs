using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Repositories
{
    public interface IInvoiceRepository : IBaseRepository
    {
        IEnumerable<Invoice?> GetInvoices();
        Task<Invoice?> GetInvoiceById(long id);

        Task<Invoice?> GetInvoiceByUserId(int userId);

        Task<Invoice> InsertInvoice(Invoice invoice);
        Invoice UpdateInvoice(Invoice invoice);
        void DeleteInvoice(int id);

        Task<bool> ChangeInvoiceState(int userId, InvoiceState newState);
    }
}