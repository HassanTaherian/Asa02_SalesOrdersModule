using Contracts.UI;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Asa02_SalesOrdersModule.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpPatch]
        public async Task<IActionResult> AddDiscountCode([FromBody]
            DiscountCodeRequestDto discountCodeRequestDto, CancellationToken cancellationToken)
        {
            await _discountService.SetDiscountCodeAsync
                (discountCodeRequestDto, cancellationToken);
            return Ok("Successful");
        }
    }
}