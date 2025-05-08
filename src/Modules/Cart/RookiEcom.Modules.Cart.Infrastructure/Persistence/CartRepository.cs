using Microsoft.EntityFrameworkCore;
using RookiEcom.Modules.Cart.Application.Abstraction;

namespace RookiEcom.Modules.Cart.Infrastructure.Persistence;

public class CartRepository : ICartRepository
{
    private readonly CartContext _cartContext;

    public CartRepository(CartContext cartContext)
    {
        _cartContext = cartContext;
    }

    public async Task<Cart.Domain.CartAggregate.Cart?> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _cartContext.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);
    }

    public async Task AddAsync(Cart.Domain.CartAggregate.Cart cart, CancellationToken cancellationToken = default)
    {
        await _cartContext.Carts.AddAsync(cart, cancellationToken);
    }

    public Task UpdateAsync(Cart.Domain.CartAggregate.Cart cart, CancellationToken cancellationToken)
    {
        _cartContext.Carts.Update(cart);
        
        return Task.CompletedTask;
    }
}