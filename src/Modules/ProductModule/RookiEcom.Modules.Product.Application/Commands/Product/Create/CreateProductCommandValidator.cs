﻿using FluentValidation;

namespace RookiEcom.Modules.Product.Application.Commands.Product.Create;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.SKU)
            .NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(100).WithMessage("SKU must not exceed 100 characters.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Category ID must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(512).WithMessage("Description must not exceed 512 characters.");

        RuleFor(x => x.MarketPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Market price must be 0 or greater.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity must be 0 or greater.");

        RuleFor(x => x.Images)
            .NotNull().WithMessage("Images array must not be null.")
            .Must(images => images != null && images.Length >= 1)
            .WithMessage($"At least {1} image is required.")
            .Must(images => images != null && images.Length <= 5)
            .WithMessage($"Number of images must not exceed {5}.")
            .Must(images => images == null || images.Length == 0 || 
                            images.All(image => image != null && image.ContentType.StartsWith("image/")))
            .WithMessage("All uploaded files must be images (e.g., JPEG, PNG).");

        RuleFor(x => x.ProductAttributes)
            .NotNull().WithMessage("Product attributes must not be null.")
            .DependentRules(() =>
            {
                RuleFor(x => x.ProductAttributes)
                    .Must(attrs => attrs.Count >= 1)
                    .WithMessage("Product must have at least 1 attribute.");
            });

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