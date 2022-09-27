using Contracts.UI.Watch;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Asa02_SalesOrdersModule.Controllers
{
    [ApiController, Route("api/[controller]/[action]")]
    public class WatchController : Controller
    {
        private readonly IWatchService _watchService;

        public WatchController(IWatchService watchService)
        {
            _watchService = watchService;
        }

        [HttpGet]
        public async Task<IActionResult> ExistedCartItems(WatchRequestItemsDto watchRequestItemsDto)
        {
            var items = await _watchService.ExistedCartItems(watchRequestItemsDto);
            return Ok(items);
        }

        [HttpGet]
        public async Task<IActionResult> IsDeletedCartItems(WatchRequestItemsDto watchRequestItemsDto)
        {
            var items = await _watchService.IsDeletedCartItems(watchRequestItemsDto);
            return Ok(items);
        }

        [HttpGet]
        public  IActionResult OrderInvoices(WatchRequestItemsDto watchRequestItemsDto)
        {
            var invoices = _watchService.OrderInvoices(watchRequestItemsDto);
            return Ok(invoices);
        }

        [HttpGet]
        public IActionResult ReturnInvoices(WatchRequestItemsDto watchRequestItemsDto)
        {
            var invoices = _watchService.ReturnInvoices(watchRequestItemsDto);
            return Ok(invoices);
        }

        [HttpGet]
        public async Task<IActionResult> ShoppedInvoiceItems(WatchInvoicesRequestDto watchInvoicesRequestDto)
        {
            var items = await _watchService.ShoppedInvoiceItems(watchInvoicesRequestDto);
            return Ok(items);
        }

        [HttpGet]
        public IActionResult ReturnedInvoiceItems(WatchInvoicesRequestDto watchInvoicesRequestDto)
        {
            var items = _watchService.ReturnedInvoiceItems(watchInvoicesRequestDto);
            return Ok(items);
        }
    }
}
