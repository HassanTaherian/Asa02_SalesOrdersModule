using Contracts.UI.Cart;

namespace Contracts.UI
{
    public class ProductToSecondCartRequestDto
    {
        public long InvoiceId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}