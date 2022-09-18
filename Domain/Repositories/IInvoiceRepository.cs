using Domain.Entities;

namespace Domain.Repositories
{
    public interface IInvoiceRepository
    {
        Task<IEnumerable<Invoice?>> GetInvoices();
        Task<Invoice?> GetInvoiceById(int id);
        Task<Invoice?> AddInvoice(Invoice invoice);
        Task<Invoice?> UpdateInvoice(Invoice invoice);
        Task DeleteInvoice(int id);
    }
}
