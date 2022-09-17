namespace Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public long ProductId { get; set; }

        public short Quantity { get; set; }

        public double ProductPrice { get; set; }

        public virtual Cart Cart { get; set; }
    }
}