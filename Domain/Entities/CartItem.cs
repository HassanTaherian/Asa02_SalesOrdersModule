namespace Domain.Entities
{
    public abstract class CartItem : BaseEntity
    {
        public long ProductId { get; set; }

        public virtual Cart Cart { get; set; }
    }
}