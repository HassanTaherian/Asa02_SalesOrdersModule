using System;
using Domain.Exceptions;

namespace Domain.Exceptions
{
    public sealed class CartOfUserNotFoundException: NotFoundException
    {
        public CartOfUserNotFoundException(int userId)
            : base($"The Cart with the user identifier {userId} was not found.")
        {
        }
    }
}
