namespace Domain.Entities
{
    public abstract class CartItem : BaseEntity
    {
        public long ProductId { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public Cart Cart { get; set; }
    }
}
