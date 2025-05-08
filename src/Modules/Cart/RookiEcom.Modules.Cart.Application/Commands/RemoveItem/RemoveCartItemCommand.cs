using RookiEcom.Application.Contracts;

namespace RookiEcom.Modules.Cart.Application.Commands.RemoveItem;

public sealed class RemoveCartItemCommand(Guid customerId, int cartItemId) : CommandBase
{
    public Guid CustomerId { get; set; } = customerId;
    public int CartItemId { get; set; } = cartItemId;
}