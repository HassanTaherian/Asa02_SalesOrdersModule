using Contracts.UI;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Services.Services;

namespace Asa02_SalesOrdersModule.Controllers
{
    [ApiController , Route("api/[controller]")]
    public class SecondCartController : ControllerBase
    {
        private readonly ISecondCardService _secondCardService;

        public SecondCartController(ISecondCardService secondCardService)
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
    }
}
