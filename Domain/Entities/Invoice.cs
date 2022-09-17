using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public long UserId { get; set; }

        public double TotalPrice { get; set; }

        public double NewTotalPrice { get; set; }

        public virtual double DifferencePrice { get; set; }

        public InvoiceState State { get; set; }

        public string DiscountCode { get; set; }

        public DateTime ShoppingDateTime { get; set; }

        public Address Address { get; set; }

        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}