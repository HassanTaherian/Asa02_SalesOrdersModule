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

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutRequestDto checkout)
        {
            var result = await _orderService.Checkout(checkout);

            if (result)
            {
                return Ok();
            }

            // Todo: Return 404 in case of error
            return NotFound();
        }
    }
}
