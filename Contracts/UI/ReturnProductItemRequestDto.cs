namespace Contracts.UI
{
    public class ReturningRequestDto
    {
        public long InvoiceId { get; set; }
        public ICollection<int> ProductIds { get; set; }
    }
}
