using Contracts.UI.Checkout;

namespace Services.Abstractions
{
    public interface IOrderService
    {
        Task<bool> Checkout(CheckoutRequestDto dto);
    }
}
