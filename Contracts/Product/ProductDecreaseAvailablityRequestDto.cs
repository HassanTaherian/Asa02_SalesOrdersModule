namespace Contracts.Product
{
    public class ProductDecreaseAvailabilityRequestDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
