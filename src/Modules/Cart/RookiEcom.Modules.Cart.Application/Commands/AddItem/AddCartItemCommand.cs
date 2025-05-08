using RookiEcom.Application.Contracts;

namespace RookiEcom.Modules.Cart.Application.Commands.AddItem;

public sealed class AddCartItemCommand : CommandBase
{
    public Guid CustomerId { get; set; }
    public string ProductName { get; set; }
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Image { get; set; }
}