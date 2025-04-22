using FluentValidation;

namespace RookiEcom.Modules.Product.Application.Commands.Category.Update;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Category ID must be greater than 0.");

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