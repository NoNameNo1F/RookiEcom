using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Moq;
using RookiEcom.Modules.Product.Application.Commands.ProductRating.Create;
using RookiEcom.Modules.Product.Application.UnitTests.Abstractions;

namespace RookiEcom.Modules.Product.Application.UnitTests.ProductRatings;

public class ProductRatingCommandTests : BaseServiceTest
{
    [Fact]
    public async Task CreateProductRatingValidator_ValidCommand_ShouldPass()
    {
        // Arrange
        var (scope, validator) = GetScopedService<CreateProductRatingCommandValidator>();
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.ContentType).Returns("image/jpeg");
        mockFile.Setup(f => f.Length).Returns(1024 * 1024);
        
        var command = new CreateProductRatingCommand
        {
            ProductId = 1,
            CustomerId = Guid.NewGuid(),
            CustomerName = "John Doe",
            Score = 4,
            Content = "Great product!",
            Image = mockFile.Object
        };
        
        // Act
        var result = await validator.TestValidateAsync(command);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task CreateProductRatingValidator_InvalidCommand_ShouldHaveValidationErrors()
    {
        // Arrange
        var (scope, validator) = GetScopedService<CreateProductRatingCommandValidator>();
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.ContentType).Returns("application/pdf");
        mockFile.Setup(f => f.Length).Returns(3 * 1024 * 1024);
        
        var command = new CreateProductRatingCommand
        {
            ProductId = 0,
            CustomerId = Guid.Empty,
            CustomerName = "",
            Score = 0,
            Content = "",
            Image = mockFile.Object
        };

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.ProductId)
            .WithErrorMessage("Product ID must be valid.");
        result.ShouldHaveValidationErrorFor(c => c.CustomerId)
            .WithErrorMessage("Customer ID is required.");
        result.ShouldHaveValidationErrorFor(c => c.CustomerName)
            .WithErrorMessage("Customer name is required.");
        result.ShouldHaveValidationErrorFor(c => c.Score)
            .WithErrorMessage("Rating score must be between 1 and 5.");
        result.ShouldHaveValidationErrorFor(c => c.Content)
            .WithErrorMessage("Review content cannot be empty.");
        result.ShouldHaveValidationErrorFor(c => c.Image)
            .WithErrorMessage("Uploaded file must be an image (e.g., JPEG, PNG).");
        result.ShouldHaveValidationErrorFor(c => c.Image)
            .WithErrorMessage("Image size cannot exceed 2MB.");
    }
}