using RookiEcom.Application.Abstraction;
using RookiEcom.Application.Contracts;
using RookiEcom.Modules.Cart.Application.Abstraction;

namespace RookiEcom.Modules.Cart.Application.Commands.ClearCart;

public class ClearCartCommandHandler : ICommandHandler<ClearCartCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ClearCartCommandHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId, cancellationToken);
        if (cart == null)
        {
            return;
        }
        
        cart.ClearItems();

        await _cartRepository.UpdateAsync(cart, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}