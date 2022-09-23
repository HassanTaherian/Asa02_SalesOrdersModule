using System.Collections;
using Contracts.UI;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Asa02_SalesOrdersModule.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SecondCartController : ControllerBase
    {
        private readonly ISecondCartService _secondCardService;

        public SecondCartController(ISecondCartService secondCardService)
        {
            _secondCardService = secondCardService;
        }

        [HttpGet]
        public async Task<IEnumerable?> GetSecondCartItems(
            [FromRoute] ProductToSecondCartResponseDto productToSecondCartResponseDto)
        {
            return await _secondCardService.GetSecondCartItems(productToSecondCartResponseDto);
        }

        [HttpPatch]
        public async Task<IActionResult> PutItemInTheSecondCart(
            [FromBody] ProductToSecondCartRequestDto productToSecondCardRequestDto)
        {
            await _secondCardService.CartToSecondCart(productToSecondCardRequestDto);
            return Ok("Successful");
        }

        [HttpPatch]
        public async Task<IActionResult> BackItemToTheCart(
            [FromBody] ProductToSecondCartRequestDto productToSecondCardRequestDto)
        {
            await _secondCardService.SecondCartToCart
                (productToSecondCardRequestDto);
            return Ok("Successful");
        }

        [HttpDelete]
        public async Task DeleteItemFromSecondList(
            [FromBody] ProductToSecondCartRequestDto productToSecondCardRequestDto)
        {
            await _secondCardService.DeleteItemFromTheSecondList(productToSecondCardRequestDto);
        }
    }
}