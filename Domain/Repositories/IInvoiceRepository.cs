﻿using Domain.Entities;
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

        Task<IEnumerable<InvoiceItem>?> GetItemsOfCart(int userId, bool isInSecondCart,bool isDeleted);

        Task<IEnumerable<InvoiceItem>> GetNotDeleteItems(long invoiceId);

        Task<bool> UserHasAnyInvoice(int userId);

        IList<int> MostFrequentShoppedProducts();

        Task FromCartToTheSecondCart(long invoiceId, int productId);

        Task FromSecondCartToTheCart(long invoiceId, int productId);

        Task DeleteItemFromTheSecondCart(long invoiceId, int productId);
    }
}