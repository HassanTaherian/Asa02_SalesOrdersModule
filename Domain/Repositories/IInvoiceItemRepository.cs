﻿using Domain.Entities;

namespace Domain.Repositories
{
    public interface IInvoiceItemRepository
    {
        Task<IEnumerable<InvoiceItem?>> GetInvoiceItems();
        Task<InvoiceItem?> GetInvoiceItemById(int id);
        Task<InvoiceItem?> AddInvoiceItem(InvoiceItem invoiceItem);
        Task<InvoiceItem?> UpdateInvoiceItem(InvoiceItem invoiceItem);
        Task DeleteInvoiceItem(int id);
    }
}