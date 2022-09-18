namespace Contracts.Marketing
{
    public class MarketingInvoiceRequest
    {
        public long InvoiceId { get; set; }
        public int UserId { get; set; }
        public string InvoiceState { get; set; }
        public DateTime ShopDateTime { get; set; }
    }
}
