using RookiEcom.Application.Abstraction;
using RookiEcom.Application.Contracts;
using RookiEcom.Modules.Cart.Application.Abstraction;
using RookiEcom.Modules.Cart.Application.Exceptions;

namespace RookiEcom.Modules.Cart.Application.Commands.UpdateQuantity;

public class UpdateCartItemQuantityCommandHandler : ICommandHandler<UpdateCartItemQuantityCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCartItemQuantityCommandHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId, cancellationToken);
        if (cart == null)
        {
            throw new CartNotFoundException(request.CustomerId);
        }

        cart.UpdateItemQuantity(request.CartItemId, request.NewQuantity);

        await _cartRepository.UpdateAsync(cart, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}