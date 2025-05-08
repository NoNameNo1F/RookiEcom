using RookiEcom.Application.Abstraction;
using RookiEcom.Application.Contracts;
using RookiEcom.Modules.Cart.Application.Abstraction;

namespace RookiEcom.Modules.Cart.Application.Commands.AddItem;

public class AddCartItemCommandHandler : ICommandHandler<AddCartItemCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddCartItemCommandHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId, cancellationToken);
        bool isNewCart = cart == null;

        if (isNewCart)
        {
            cart = new Domain.CartAggregate.Cart(request.CustomerId);
        }

        cart!.AddItem(
            request.ProductId,
            request.ProductName,
            request.Quantity,
            request.Price,
            request.Image);

        if (isNewCart)
        {
            await _cartRepository.AddAsync(cart, cancellationToken);
        }
        else
        {
            await _cartRepository.UpdateAsync(cart, cancellationToken);
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}