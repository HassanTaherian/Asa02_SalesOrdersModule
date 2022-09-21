using System.Collections;
using Contracts.UI;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Asa02_SalesOrdersModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecondCartController : ControllerBase
    {
        private readonly ISecondCartService _secondCardService;

        public SecondCartController(ISecondCartService secondCardService)
        {
            _secondCardService = secondCardService;
        }


        [HttpGet]
        public async Task<IEnumerable?> GetSecondCartItems
            (ProductToSecondCartResponseDto productToSecondCartResponseDto)
        {
            return await _secondCardService.GetSecondCartItems(productToSecondCartResponseDto);
        }


        [HttpPatch]
        public async Task<IActionResult> ToggleItemBetweenCarts(
            [FromBody] ProductToSecondCartRequestDto productToSecondCardRequestDto
            , CancellationToken cancellationToken)
        {
            await _secondCardService.ToggleItemInTheCart
                (productToSecondCardRequestDto, cancellationToken);
            return Ok("Successful");
        }


        [HttpDelete]
        public async Task DeleteItemFromSecondList(
            [FromBody] ProductToSecondCartRequestDto productToSecondCardRequestDto
            , CancellationToken cancellationToken)
        {
            await _secondCardService.DeleteItemFromTheSecondList
                (productToSecondCardRequestDto, cancellationToken);
        }
    }
}
