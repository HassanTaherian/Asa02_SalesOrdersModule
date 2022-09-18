using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public long UserId { get; set; }

        public InvoiceState State { get; set; }

        public string DiscountCode { get; set; }

        public DateTime ShoppingDateTime { get; set; }

        public int AddressId { get; set; }

        public ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}