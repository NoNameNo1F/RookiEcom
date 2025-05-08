using RookiEcom.Application.Abstraction;
using RookiEcom.Application.Contracts;
using RookiEcom.Modules.Cart.Application.Abstraction;
using RookiEcom.Modules.Cart.Application.Exceptions;

namespace RookiEcom.Modules.Cart.Application.Commands.RemoveItem;

public class RemoveCartItemCommandHandler : ICommandHandler<RemoveCartItemCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveCartItemCommandHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId, cancellationToken);
        if (cart == null)
        {
            throw new CartNotFoundException(request.CustomerId);
        }
        
        cart.RemoveItem(request.CartItemId);

        await _cartRepository.UpdateAsync(cart, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}