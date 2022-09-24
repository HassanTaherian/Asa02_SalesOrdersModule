namespace Domain.Exceptions
{
    public class InvoiceNotFoundException : NotFoundException
    {
        public InvoiceNotFoundException(long invoiceId) : base($"The invoice with id {invoiceId} not found!")
        {
        }
    }
}
