using Contracts.UI.Cart;
using Contracts.UI.Watch;
using Domain.Repositories;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Asa02_SalesOrdersModule.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CartController : Controller
    {
        private readonly IProductService _productService;

        public CartController(IProductService productService)
        {
            _productService = productService;
        }
        
        // Todo: Remove WatchRequestItemsDto and Get UserId from Route
        [HttpGet]
        public async Task<IActionResult> ExistedCartItems(WatchRequestItemsDto watchRequestItemsDto)
        {
            var items = await _productService.ExistedCartItems(watchRequestItemsDto);
            return Ok(items);
        }

        [HttpGet]
        public IActionResult IsDeletedCartItems(WatchRequestItemsDto watchRequestItemsDto)
        {
            var items = _productService.IsDeletedCartItems(watchRequestItemsDto);
            return Ok(items);
        }


        // POST: CartController/Create
        [HttpPost]
        public async Task AddProduct(AddProductRequestDto addProductRequestDto)
        {
            await _productService.AddCart(addProductRequestDto, InvoiceState.CartState);
        }

        // PATCH: CartController/Update
        [HttpPatch]
        public async Task UpdateProduct(UpdateQuantityRequestDto updateQuantityRequestDto)
        {
            await _productService.UpdateQuantity(updateQuantityRequestDto);
        }

        // DELETE: CartController/Delete
        [HttpDelete]
        public async Task DeleteProduct(DeleteProductRequestDto deleteProductRequestDto)
        {
            await _productService.DeleteItem(deleteProductRequestDto);
        }
    }
}