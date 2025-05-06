using FluentValidation;

namespace RookiEcom.Modules.Product.Application.Commands.ProductRating.Create;

public class CreateProductRatingCommandValidator : AbstractValidator<CreateProductRatingCommand>
{
    public CreateProductRatingCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Product ID must be valid.");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("Customer name is required.")
            .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters.");
        
        RuleFor(x => x.Score)
            .Must(score => score >= 1 && score <= 5)
            .WithMessage("Rating score must be between 1 and 5.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Review content cannot be empty.")
            .MaximumLength(512).WithMessage("Review content cannot exceed 512 characters.");


        RuleFor(x => x.Image)
            .Must(image => image == null || image.ContentType.StartsWith("image/"))
            .WithMessage("Uploaded file must be an image (e.g., JPEG, PNG).")
            .Must(image => image == null || image.Length <= 2 * 1024 * 1024)
            .WithMessage("Image size cannot exceed 2MB.")
            .When(x => x.Image != null);
    }
}