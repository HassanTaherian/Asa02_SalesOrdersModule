using System.Collections;
using Contracts.UI;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Asa02_SalesOrdersModule.Controllers
{
    [ApiController]
    public class SecondCartController : ControllerBase
    {
        private readonly ISecondCartService _secondCardService;

        public SecondCartController(ISecondCartService secondCardService)
        {
            _secondCardService = secondCardService;
        }


        [HttpGet , Route("api/[controller]/get")]
        public async Task<IEnumerable?> GetSecondCartItems
            (ProductToSecondCartResponseDto productToSecondCartResponseDto)
        {
            return await _secondCardService.GetSecondCartItems(productToSecondCartResponseDto);
        }


        [HttpPatch , Route("api/[controller]/putItem")]
        public async Task<IActionResult> AddItemToSecondCart(
            [FromBody] ProductToSecondCartRequestDto productToSecondCardRequestDto
            , CancellationToken cancellationToken)
        {
            await _secondCardService.PutItemInTheSecondCard
                (productToSecondCardRequestDto, cancellationToken);
            return Ok("Successful");
        }


        [HttpPatch , Route("api/[controller]/backItem")]
        public async Task<IActionResult> BackItemToTheCart(
            [FromBody] ProductToSecondCartRequestDto productToSecondCardRequestDto
            , CancellationToken cancellationToken)
        {
            await _secondCardService.BackItemToTheCart
                (productToSecondCardRequestDto, cancellationToken);
            return Ok();
        }


        [HttpDelete , Route("api/[controller]/remove")]
        public async Task DeleteItemFromSecondList(
            [FromBody] ProductToSecondCartRequestDto productToSecondCardRequestDto
            , CancellationToken cancellationToken)
        {
            await _secondCardService.DeleteItemFromTheSecondList
                (productToSecondCardRequestDto, cancellationToken);
        }
    }
}
