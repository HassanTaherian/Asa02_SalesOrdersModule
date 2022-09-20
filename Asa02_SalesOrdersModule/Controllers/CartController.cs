using Contracts.UI.Cart;
using Domain.Repositories;
using Domain.ValueObjects;
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

        public CartController(IProductService productService)
        {
            _productService = productService;
        }

        // POST: CartController/Create
        [HttpPost]
        public async Task AddProduct(AddProductRequestDto addProductRequestDto)
        {
            await _productService.AddCart(addProductRequestDto, InvoiceState.CartState, CancellationToken.None);
        }

        // PATCH: CartController/Update
        [HttpPatch]
        public async Task UpdateProduct(UpdateQuantityRequestDto updateQuantityRequestDto)
        {
            await _productService.UpdateQuantity(updateQuantityRequestDto, CancellationToken.None);
        }

        // DELETE: CartController/Delete
        [HttpDelete]
        public async Task DeleteProduct(DeleteProductRequestDto deleteProductRequestDto)
        {
            await _productService.DeleteItem(deleteProductRequestDto);
        }
    }
}