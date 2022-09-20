using Contracts.UI;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Asa02_SalesOrdersModule.Controllers
{
    [ApiController, Route("api/[controller]/[action]")]
    public class AdditionalController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        private readonly IAddressService _addressService;

        public AdditionalController(IDiscountService discountService,
            IAddressService addressService)
        {
            _discountService = discountService;
            _addressService = addressService;
        }

        [HttpPatch]
        public async Task<IActionResult> AddDiscountCode(
            [FromBody] AdditionalInvoiceDataDto additionalInvoiceDataDto)
        {
            await _discountService.SetDiscountCodeAsync
                (additionalInvoiceDataDto , CancellationToken.None);
            return Ok("Successful");
        }

         [HttpPatch]
         public async Task<IActionResult> AddAddressCode(
             [FromBody] AdditionalInvoiceDataDto additionalInvoiceDataDto)
         {
             await _addressService.SetAddressIdAsync
                 (additionalInvoiceDataDto , CancellationToken.None);
             return NoContent();
         }
    }

}
        