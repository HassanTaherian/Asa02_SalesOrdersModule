using Contracts.UI.Cart;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Asa02_SalesOrdersModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        private readonly IProductService _productService;

        // POST: CartController/Create
        [HttpPost]
        public async Task AddProduct(AddProductRequestDto addProductRequestDto)
        {
            await _productService.Add(addProductRequestDto);
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
            await _productService.DeleteItem();
        }
    }
}