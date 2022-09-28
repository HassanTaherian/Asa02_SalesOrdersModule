using System.Collections;
using Contracts.UI;
using Domain.Entities;

namespace Services.Abstractions
{
    public interface ISecondCartService
    {
        IEnumerable<InvoiceItem> GetSecondCartItems(int userId);

        Task SecondCartToCart
            (ProductToSecondCartRequestDto productToSecondCartRequestDto);

        Task CartToSecondCart
            (ProductToSecondCartRequestDto productToSecondCartRequestDto);

        Task DeleteItemFromTheSecondCart
            (ProductToSecondCartRequestDto productToSecondCartRequestDto);
    }
}