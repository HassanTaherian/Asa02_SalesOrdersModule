namespace Domain.Entities
{
    public class WishList : BaseEntity
    {
        public long UserId { get; set; }

        public bool IsUserDefined { get; set; }

        public virtual ICollection<WishListItem>? WishListItems { get; set; }
    }
}