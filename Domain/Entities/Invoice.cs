namespace Domain.Entities
{
    public class Invoice
    {
        public long InvoiceId { get; set; }

        public long UserId { get; set; }

        public double TotalPrice { get; set; }

        public bool State { get; set; }

        public string DiscountCode { get; set; }

        public DateTime ShoppingDateTime { get; set; }

        public Address Address { get; set; }

        public virtual ICollection<InvoiceItem> Items { get; set; }

    }
}