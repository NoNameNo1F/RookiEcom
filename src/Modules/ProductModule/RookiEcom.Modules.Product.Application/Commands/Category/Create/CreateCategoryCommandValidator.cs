using FluentValidation;

namespace RookiEcom.Modules.Product.Application.Commands.Category.Create;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(x => x.ParentId)
            .GreaterThanOrEqualTo(0).WithMessage("Parent ID must be 0 or greater.")
            .When(x => x.ParentId.HasValue);

        RuleFor(x => x.Image)
            .Must(image => image == null || image.ContentType.StartsWith("image/"))
            .WithMessage("Uploaded file must be an image (e.g., JPEG, PNG).");
    }
}