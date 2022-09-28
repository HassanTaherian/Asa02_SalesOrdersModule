using Contracts.UI;
using Contracts.UI.Checkout;
using Contracts.UI.Watch;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Asa02_SalesOrdersModule.Controllers
{
    [ApiController, Route("/api/[controller]/[action]")]
    public class ReturningController : Controller
    {
        private readonly IReturningService _returningService;

        public ReturningController(IReturningService returningService)
        {
            _returningService = returningService;
        }

        [HttpGet]
        public IActionResult ReturnInvoices(WatchRequestItemsDto watchRequestItemsDto)
        {
            var invoices = _returningService.ReturnInvoices(watchRequestItemsDto);
            return Ok(invoices);
        }
        
        [HttpGet]
        public IActionResult ReturnedInvoiceItems(WatchInvoicesRequestDto watchInvoicesRequestDto)
        {
            var items = _returningService.ReturnedInvoiceItems(watchInvoicesRequestDto);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Return(ReturningRequestDto returningRequestDto)
        {
            await _returningService.Return(returningRequestDto);
            return Ok();
        }
    }
}