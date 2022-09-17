namespace Domain.Entities
{
    public class WishListItem : BaseEntity
    {
        public long ProductId { get; set; }

        public double ProductPrice { get; set; }

        public virtual WishList WishList { get; set; }
    }
}