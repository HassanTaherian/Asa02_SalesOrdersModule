namespace Domain.Entities
{
    public class WishList
    {
        public int Id { get; set; }

        public long UserId { get; set; }

        public virtual ICollection<WishListItem>? WishListItems { get; set; }
    }
}