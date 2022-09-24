using Contracts.UI;
using Contracts.UI.Checkout;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;
using Services.Abstractions;

namespace Asa02_SalesOrdersModule.Controllers
{
    [ApiController, Route("/api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost, Route("Checkout")]
        public async Task<IActionResult> Checkout(CheckoutRequestDto checkout)
        {
            await _orderService.Checkout(checkout);
            return Ok();
        }

        [HttpPost, Route("Returning")]
        public async Task<IActionResult> Returning(ReturningRequestDto returningRequestDto)
        {
            await _orderService.Returning(returningRequestDto);
            return Ok();
        }
    }
}
