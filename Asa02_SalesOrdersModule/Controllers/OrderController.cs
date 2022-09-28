using Contracts.UI;
using Contracts.UI.Checkout;
using Contracts.UI.Watch;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Asa02_SalesOrdersModule.Controllers
{
    [ApiController, Route("/api/[controller]/[action]")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        
        [HttpGet]
        public  IActionResult OrderInvoices(WatchRequestItemsDto watchRequestItemsDto)
        {
            var invoices = _orderService.OrderInvoices(watchRequestItemsDto);
            return Ok(invoices);
        }
        
        [HttpGet]
        public async Task<IActionResult> ShoppedInvoiceItems(WatchInvoicesRequestDto watchInvoicesRequestDto)
        {
            var items = await _orderService.ShoppedInvoiceItems(watchInvoicesRequestDto);
            return Ok(items);
        }
        
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutRequestDto checkout)
        {
            await _orderService.Checkout(checkout);
            return Ok();
        }
    }
}
