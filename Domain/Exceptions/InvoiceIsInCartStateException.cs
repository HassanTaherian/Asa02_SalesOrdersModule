namespace Domain.Exceptions
{
    public class InvoiceIsInCartStateException : BadRequestException
    {
        public InvoiceIsInCartStateException(long invoiceId) : base($"Invoice {invoiceId} currently at Cart State!")
        {
        }
    }
}
