using FluentValidation;

namespace RookiEcom.Modules.Product.Application.Commands.Product.Update;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Product ID must be greater than 0.");

        RuleFor(x => x.SKU)
            .NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(100).WithMessage("SKU must not exceed 100 characters.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Category ID must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(512).WithMessage("Description must not exceed 512 characters.");

        RuleFor(x => x.MarketPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Market price must be 0 or greater.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity must be 0 or greater.");
        
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid product status provided.");

        RuleFor(x => x.OldImages)
            .Must(images => images != null && images.Count >= 1)
            .WithMessage("Old images must contain at least one image.");
        
        RuleForEach(x => x.OldImages)
            .Must(url => !string.IsNullOrEmpty(url))
            .WithMessage("Old image URL must not be empty.");
        
        RuleForEach(x => x.NewImages)
            .Must(image => image == null || image.ContentType.StartsWith("image/"))
            .When(x => x.NewImages != null && x.NewImages.Any())
            .WithMessage("All uploaded files must be images (e.g., JPEG, PNG).");
        
        RuleFor(x => x)
            .Custom((command, context) => {
                int oldImageCount = command.OldImages?.Count ?? 0;
                int newImageCount = command.NewImages?.Length ?? 0;
                int totalImages = oldImageCount + newImageCount;

                if (totalImages < 1)
                {
                    context.AddFailure(nameof(command.OldImages), "Product must have at least one image (existing or new).");
                    context.AddFailure(nameof(command.NewImages), "Product must have at least one image (existing or new).");
                }

                if (totalImages > 5)
                {
                    context.AddFailure(nameof(command.OldImages), "Total number of images (existing + new) must not exceed 5.");
                    context.AddFailure(nameof(command.NewImages), "Total number of images (existing + new) must not exceed 5.");
                }
            });
        
        RuleFor(x => x.ProductAttributes)
            .Must(attrs => attrs != null && attrs.Count >= 1)
            .WithMessage("Product must have at least 1 attribute.");
        
        RuleForEach(x => x.ProductAttributes)
            .ChildRules(attr =>
            {
                attr.RuleFor(a => a.Code)
                    .NotEmpty().WithMessage("Product attribute code is required.")
                    .MaximumLength(50).WithMessage("Product attribute code must not exceed 50 characters.");

                attr.RuleFor(a => a.Value)
                    .NotEmpty().WithMessage("Product attribute value is required.")
                    .MaximumLength(200).WithMessage("Product attribute value must not exceed 200 characters.");
            });

        RuleFor(x => x.ProductOption)
            .NotNull().WithMessage("Product options must not be null.")
            .ChildRules(opt =>
            {
                opt.RuleFor(o => o.Code)
                    .NotEmpty().WithMessage("Product option code is required.")
                    .MaximumLength(50).WithMessage("Product option code must not exceed 50 characters.");

                opt.RuleFor(o => o.Values)
                    .NotEmpty().WithMessage("Product option values must not be empty.")
                    .Must(values => values.All(v => !string.IsNullOrEmpty(v) && v.Length <= 100))
                    .WithMessage("Each product option value must not be empty and must not exceed 100 characters.");
            });
    }
}