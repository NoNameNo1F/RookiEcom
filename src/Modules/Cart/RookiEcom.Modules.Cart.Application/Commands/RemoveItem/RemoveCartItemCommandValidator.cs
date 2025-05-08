using FluentValidation;

namespace RookiEcom.Modules.Cart.Application.Commands.RemoveItem;

public class RemoveCartItemCommandValidator : AbstractValidator<RemoveCartItemCommand>
{
    public RemoveCartItemCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.CartItemId)
            .GreaterThan(0).WithMessage("Cart Item ID must be valid.");
    }
}