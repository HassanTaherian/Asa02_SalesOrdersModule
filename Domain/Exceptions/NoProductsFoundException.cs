namespace Domain.Exceptions
{
    public class NoProductsFoundException : NotFoundException
    {
        public NoProductsFoundException()
            : base($"There is no product in the Invoice!")
        {
        }
    }
}