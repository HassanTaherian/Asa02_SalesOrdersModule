﻿using Contracts.UI.Cart;
using Domain.ValueObjects;

namespace Services.Abstractions
{
    public interface IProductService
    {
        Task AddCart(AddProductRequestDto addProductRequestDto, InvoiceState invoiceState);
        Task UpdateQuantity(UpdateQuantityRequestDto updateQuantityRequestDto);
        Task DeleteItem(DeleteProductRequestDto deleteProductRequestDto);

    }
}
