using RookiEcom.Modules.Cart.Application.Abstraction;
using RookiEcom.Modules.Cart.Contracts.Dtos;

namespace RookiEcom.Modules.Cart.Application.Queries;

public class CartService
{
    private readonly ICartRepository _cartRepository;
        
    public CartService(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }
    
    private CartDto ToCartDto(Domain.CartAggregate.Cart cart)
    {
        return new CartDto
        {
            Id = cart.Id,
            CustomerId = cart.CustomerId,
            CartItems = cart.CartItems.Select(item => new CartItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                Price = item.Price,
                Image = item.Image,
                SubTotal = item.CalculateSubTotal()
            }).ToList(),
            TotalPrice = cart.TotalPrice
        };
    }
    
    public async Task<CartDto> GetCartByCustomerId(Guid customerId, CancellationToken cancellationToken)
    {
        var cart =  await _cartRepository.GetByCustomerIdAsync(customerId, cancellationToken);
        
        return ToCartDto(cart);
    }
}