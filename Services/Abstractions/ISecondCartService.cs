﻿using System.Collections;
using Contracts.UI;

namespace Services.Abstractions
{
    public interface ISecondCartService
    {
        Task<IEnumerable?> GetSecondCartItems(int userId);

        Task SecondCartToCart
            (ProductToSecondCartRequestDto productToSecondCartRequestDto);

        Task CartToSecondCart
            (ProductToSecondCartRequestDto productToSecondCartRequestDto);

        Task DeleteItemFromTheSecondCart
            (ProductToSecondCartRequestDto productToSecondCartRequestDto);
    }
}