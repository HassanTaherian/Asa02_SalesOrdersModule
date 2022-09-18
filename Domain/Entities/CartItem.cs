using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public long ProductId { get; set; }

        public short Quantity { get; set; }

        public double ProductPrice { get; set; }
        
       // [ForeignKey(nameof(xxId))]
        public virtual Cart Cart { get; set; }
        //public int xxId { get; set; }
    }
}