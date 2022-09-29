namespace Contracts.UI.Returning
{
    public class ReturningRequestDto
    {
        public long InvoiceId { get; set; }
        public ICollection<int> ProductIds { get; set; }
    }
}