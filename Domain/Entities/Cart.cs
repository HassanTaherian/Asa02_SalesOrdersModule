namespace Domain.Entities
{
    public class Cart
    {
        public long CartId { get; set; }

        public long UserId { get; set; }

        public double TotalPrice { get; set; }

        public virtual ICollection<CartItem> Items { get; set; }
    }
}
