using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Persistence.Repositories
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
