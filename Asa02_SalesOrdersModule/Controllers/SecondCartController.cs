using System.Collections;
using Contracts.UI;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Services.Services;

namespace Asa02_SalesOrdersModule.Controllers
{
    [ApiController , Route("api/[controller]")]
    public class SecondCartController : ControllerBase
    {
        private readonly ISecondCartService _secondCardService;

        public SecondCartController(ISecondCartService secondCardService)
        {
            _secondCardService = secondCardService;
        }

        [HttpPatch]
        public async Task<IActionResult> AddItemToSecondCart(
            [FromBody] ProductToSecondCartRequestDto productToSecondCardRequestDto
            , CancellationToken cancellationToken)
        {
            await _secondCardService.PutItemInTheSecondCard
                (productToSecondCardRequestDto, cancellationToken);
            return Ok("Successful");
        }

        [HttpGet]
        public async Task<IEnumerable?> GetSecondCartItems
            (ProductToSecondCartResponseDto productToSecondCartResponseDto)
        {
           return await _secondCardService.GetSecondCartItems(productToSecondCartResponseDto);
        }

    }
}
