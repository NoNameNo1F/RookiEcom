using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Moq;
using RookiEcom.Modules.Product.Application.Commands.Category.Create;
using RookiEcom.Modules.Product.Application.Commands.Category.Delete;
using RookiEcom.Modules.Product.Application.Commands.Category.Update;
using RookiEcom.Modules.Product.Application.UnitTests.Abstractions;
using RookiEcom.Modules.Product.Application.UnitTests.Utils;

namespace RookiEcom.Modules.Product.Application.UnitTests.Categories;

public class CategoryCommandTests : BaseServiceTest
{
    [Fact]
    public async Task CreateCategoryValidator_ValidCommand_ShouldPass()
    {
        // Arrange
        var (scope, validator) = GetScopedService<CreateCategoryCommandValidator>();
        var mockFile = MockFileHelper.CreateMockFormFile(fileName: "category.jpg");
        var command = new CreateCategoryCommand(
            name: "CategoryTest",
            description: "Category Description",
            parentId: null,
            isPrimary: true,
            image: mockFile.Object);
        
        // Act
        var result = await validator.TestValidateAsync(command);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task CreateCategoryValidator_InvalidCommand_ShouldHaveValidationErrors()
    {
        // Arrange
        var (scope, validator) = GetScopedService<CreateCategoryCommandValidator>();
        var mockFile = MockFileHelper.CreateMockFormFile(fileName: "category.pdf", contentType:"application/pdf");
        
        var command = new CreateCategoryCommand(
            name: "",
            description: new string('A', 1001),
            parentId: -1,
            isPrimary: true,
            image: mockFile.Object);
        
        // Act
        var result = await validator.TestValidateAsync(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name is required.");
        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage("Description must not exceed 1000 characters.");
        result.ShouldHaveValidationErrorFor(c => c.ParentId)
            .WithErrorMessage("Parent ID must be 0 or greater.");
        result.ShouldHaveValidationErrorFor(c => c.Image)
            .WithErrorMessage("Uploaded file must be an image (e.g., JPEG, PNG).");
    }
    
    [Fact]
    public async Task UpdateCategoryValidator_ValidCommand_ShouldPass()
    {
        // Arrange
        var (scope, validator) = GetScopedService<UpdateCategoryCommandValidator>();
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.ContentType).Returns("image/jpeg");

        var command = new UpdateCategoryCommand(
            id: 1,
            name: "Test Category",
            description: "Test Description",
            parentId: null,
            isPrimary: true,
            image: mockFile.Object
        );

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task UpdateCategoryValidator_InvalidCommand_ShouldHaveValidationErrors()
    {
        // Arrange
        var (scope, validator) = GetScopedService<UpdateCategoryCommandValidator>();
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.ContentType).Returns("application/pdf");

        var command = new UpdateCategoryCommand(
            id: 0,
            name: "",
            description: new string('A', 201),
            parentId: -1,
            isPrimary: true,
            image: mockFile.Object
        );

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id)
            .WithErrorMessage("Category ID must be greater than 0.");
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name is required.");
        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage("Description must not exceed 200 characters.");
        result.ShouldHaveValidationErrorFor(c => c.ParentId)
            .WithErrorMessage("Parent ID must be 0 or greater.");
        result.ShouldHaveValidationErrorFor(c => c.Image)
            .WithErrorMessage("Uploaded file must be an image (e.g., JPEG, PNG).");
    }

    [Fact]
    public async Task DeleteCategoryValidator_ValidCommand_ShouldPass()
    {
        // Arrange
        var (scope, validator) = GetScopedService<DeleteCategoryCommandValidator>();
        var command = new DeleteCategoryCommand(1);

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task DeleteCategoryValidator_InvalidCommand_ShouldHaveValidationErrors()
    {
        // Arrange
        var (scope, validator) = GetScopedService<DeleteCategoryCommandValidator>();
        var command = new DeleteCategoryCommand(0);

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id)
            .WithErrorMessage("Category ID must be greater than 0.");
    }
}