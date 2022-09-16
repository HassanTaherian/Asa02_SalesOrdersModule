namespace Domain.Entities
{
    public abstract class WishListItem : BaseEntity
    {
        public long ProductId { get; set; }

        public WishList WishList { get; set; }
    }
}