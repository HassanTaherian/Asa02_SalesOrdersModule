namespace Domain.Entities
{
    public class Cart : BaseEntity
    {
        public long UserId { get; set; }

        public virtual ICollection<CartItem> Items { get; set; }
    }
}
