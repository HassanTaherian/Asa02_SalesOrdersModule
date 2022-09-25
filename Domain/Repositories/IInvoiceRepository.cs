﻿using System.Collections;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Repositories
{
    public interface IInvoiceRepository : IUnitOfWork
    {
        IEnumerable<Invoice?> GetInvoices();

        IEnumerable<Invoice?> GetInvoiceByState(int userId, InvoiceState invoiceState);

        Task<Invoice?> GetInvoiceById(long id);

        Task<Invoice?> GetCartOfUser(int userId);

        Task<Invoice> InsertInvoice(Invoice invoice);

        Invoice UpdateInvoice(Invoice invoice);

        void DeleteInvoice(int id);

        Task<bool> ChangeInvoiceState(int userId, InvoiceState newState);

        Task<InvoiceItem?> GetInvoiceItem(long invoiceId, int productId);

        Task<IEnumerable<InvoiceItem>> GetItemsOfInvoice(int userId);

        Task<IEnumerable<InvoiceItem>?> GetItemsOfCart(int userId, bool isInSecondCart);

        Task<bool> UserHasAnyInvoice(int userId);

        IList<int> MostFrequentShoppedProducts();

        Task FromCartToTheSecondCart(long invoiceId, int productId);

        Task FromSecondCartToTheCart(long invoiceId, int productId);

        Task DeleteItemFromTheSecondCart(long invoiceId, int productId);
    }
}