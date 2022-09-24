using System;
using Domain.Exceptions;

namespace Domain.Exceptions
{
    public sealed class InvoiceItemCartOfUserNotFoundException: NotFoundException
    {
        public InvoiceItemCartOfUserNotFoundException(int userId,int productId)
            : base($"The Product with product id {productId} in cart of user with user id {userId} was not found.")
        {
        }
    }
}
