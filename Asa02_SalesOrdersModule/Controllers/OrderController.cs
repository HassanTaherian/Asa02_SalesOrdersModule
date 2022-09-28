using Contracts.UI;
using Contracts.UI.Checkout;
using Contracts.UI.Watch;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;
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
        public IActionResult ReturnInvoices(WatchRequestItemsDto watchRequestItemsDto)
        {
            var invoices = _orderService.ReturnInvoices(watchRequestItemsDto);
            return Ok(invoices);
        }

        [HttpGet]
        public async Task<IActionResult> ShoppedInvoiceItems(WatchInvoicesRequestDto watchInvoicesRequestDto)
        {
            var items = await _orderService.ShoppedInvoiceItems(watchInvoicesRequestDto);
            return Ok(items);
        }

        [HttpGet]
        public IActionResult ReturnedInvoiceItems(WatchInvoicesRequestDto watchInvoicesRequestDto)
        {
            var items = _orderService.ReturnedInvoiceItems(watchInvoicesRequestDto);
            return Ok(items);
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
