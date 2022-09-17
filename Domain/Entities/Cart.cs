namespace Domain.Entities
{
    public class Cart : BaseEntity
    {
        public long UserId { get; set; }

        public double TotalPrice { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}