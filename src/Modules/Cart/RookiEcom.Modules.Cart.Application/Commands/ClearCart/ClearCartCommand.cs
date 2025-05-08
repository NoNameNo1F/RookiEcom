using RookiEcom.Application.Contracts;

namespace RookiEcom.Modules.Cart.Application.Commands.ClearCart;

public sealed class ClearCartCommand(Guid customerId) : CommandBase
{
    public Guid CustomerId { get; set; } = customerId;
}