namespace Domain.Entities
{
    public abstract class WishListItem : BaseEntity
    {
        public long ProductId { get; set; }

        public virtual WishList WishList { get; set; }
    }
}