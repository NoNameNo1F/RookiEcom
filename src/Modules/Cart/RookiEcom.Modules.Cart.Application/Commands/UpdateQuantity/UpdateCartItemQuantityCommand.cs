using RookiEcom.Application.Contracts;

namespace RookiEcom.Modules.Cart.Application.Commands.UpdateQuantity;

public sealed class UpdateCartItemQuantityCommand(Guid customerId, int cartItemId, int newQuantity) : CommandBase
{
    public Guid CustomerId { get; set; } = customerId;
    public int CartItemId { get; set; } = cartItemId;
    public int NewQuantity { get; set; } = newQuantity;
}