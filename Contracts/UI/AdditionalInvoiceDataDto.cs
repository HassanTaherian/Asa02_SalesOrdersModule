namespace Contracts.UI
{
    public class AdditionalInvoiceDataDto
    {
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public string? DiscountCode { get; set; }
    }
}
