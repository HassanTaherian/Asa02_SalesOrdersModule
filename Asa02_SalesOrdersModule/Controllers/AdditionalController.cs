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
            [FromBody]  DiscountCodeRequestDto discountCodeRequestDto)
        {
            await _discountService.SetDiscountCodeAsync
                (discountCodeRequestDto, CancellationToken.None);
            return Ok("Successful");
        }

         [HttpPatch]
         public async Task<IActionResult> AddAddressCode(
             [FromBody] AddressInvoiceDataDto addressInvoiceDataDto)
         {
             await _addressService.SetAddressIdAsync
                 (addressInvoiceDataDto, CancellationToken.None);
             return NoContent();
         }
    }

}
        