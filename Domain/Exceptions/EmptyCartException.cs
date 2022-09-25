namespace Domain.Exceptions
{
    public class EmptyCartException : BadRequestException
    {
        public EmptyCartException(long invoiceId) : base($"Cart {invoiceId} is Empty!")
        {
        }
    }
}
