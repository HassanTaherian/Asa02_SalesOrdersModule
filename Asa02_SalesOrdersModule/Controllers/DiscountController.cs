using Contracts.UI;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Asa02_SalesOrdersModule.Controllers
{
    [ApiController, Route("api/[controller]/[action]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpPatch]
        public async Task<IActionResult> AddDiscountCode(
            [FromBody] DiscountCodeRequestDto discountCodeRequestDto)
        {
            await _discountService.SetDiscountCodeAsync
                (discountCodeRequestDto, CancellationToken.None);
            return Ok("Successful");
        }
    }
}