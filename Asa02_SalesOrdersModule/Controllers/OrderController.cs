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
            var result = await _orderService.Checkout(checkout);

            if (result)
            {
                return Ok();
            }

            // Todo: Return 404 in case of error
            return NotFound();
        }

        [HttpPost, Route("Returning")]
        public async Task<IActionResult> Returning(IList<ReturnProductItemRequestDto> returningItems)
        {
            var result = await _orderService.Returning(returningItems);

            if (result)
            {
                return Ok();
            }

            // Todo: Return 404 in case of error
            return NotFound();
        }
    }
}
