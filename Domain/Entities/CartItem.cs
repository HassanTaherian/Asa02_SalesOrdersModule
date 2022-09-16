namespace Domain.Entities
{
    public abstract class CartItem : BaseEntity
    {
        public long ProductId { get; set; }

        public Cart Cart { get; set; }
    }
}
