namespace RookiEcom.Modules.Cart.Application.Exceptions;

public class CartNotFoundException : Exception
{
    public CartNotFoundException(Guid customerId) : base($"Cart not found for customer {customerId}")
    {
    }
}