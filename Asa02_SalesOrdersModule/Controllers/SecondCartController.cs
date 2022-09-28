using System.Collections;
using Contracts.UI;
using Domain.Entities;
using Domain.Exceptions;
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

        [HttpGet("{userId:int}")]
        public IEnumerable GetItemsInTheSecondCart(int userId)
        {
            var result = _secondCardService.GetSecondCartItems(userId);

            return result;
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
            await _secondCardService.SecondCartToCart(productToSecondCardRequestDto);
            return Ok();
        }

        [HttpDelete]
        public async Task DeleteItemFromSecondCart(
            [FromBody] ProductToSecondCartRequestDto productToSecondCardRequestDto)
        {
            await _secondCardService.DeleteItemFromTheSecondCart(productToSecondCardRequestDto);
        }
    }
}