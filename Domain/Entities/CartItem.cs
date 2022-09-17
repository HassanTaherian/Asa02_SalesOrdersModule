namespace Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public long ProductId { get; set; }

        public virtual Cart Cart { get; set; }
    }
}