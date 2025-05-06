using FluentValidation;
using RookiEcom.Modules.Product.Application.Commands.Category.Delete;
using RookiEcom.Modules.Product.Application.Commands.Product.Delete;

namespace RookiEcom.Modules.Product.Application.Commands.Category.Create;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Category ID must be greater than 0.");
    }
}