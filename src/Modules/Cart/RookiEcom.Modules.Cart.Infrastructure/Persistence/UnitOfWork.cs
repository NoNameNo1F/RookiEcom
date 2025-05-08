using RookiEcom.Application.Abstraction;

namespace RookiEcom.Modules.Cart.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly CartContext _cartContext;

    public UnitOfWork(CartContext cartContext)
    {
        _cartContext = cartContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _cartContext.SaveChangesAsync(cancellationToken);
    }
    
    public void Dispose()
    {
        _cartContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
