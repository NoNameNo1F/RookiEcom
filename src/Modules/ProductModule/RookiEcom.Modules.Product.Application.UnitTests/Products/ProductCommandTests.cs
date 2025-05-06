using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Moq;
using RookiEcom.Modules.Product.Application.Commands.Product.Create;
using RookiEcom.Modules.Product.Application.Commands.Product.Delete;
using RookiEcom.Modules.Product.Application.Commands.Product.Update;
using RookiEcom.Modules.Product.Application.UnitTests.Abstractions;
using RookiEcom.Modules.Product.Application.UnitTests.Utils;
using RookiEcom.Modules.Product.Domain.Shared;

namespace RookiEcom.Modules.Product.Application.UnitTests.Products;

public class ProductCommandTests : BaseServiceTest
{
    [Fact]
    public async Task CreateProductValidator_ValidCommand_ShouldPass()
    {
        // Arrange
        var (scope, validator) = GetScopedService<CreateProductCommandValidator>();
        var mockFile1 = MockFileHelper.CreateMockFormFile(fileName: "product1.jpg");
        var mockFile2 = MockFileHelper.CreateMockFormFile(fileName: "product2.jpg");
        var command = new CreateProductCommand(
            sku: "TEST-SKU-01",
            categoryId: 1,
            name: "Test Product",
            description: "Test Desc",
            marketPrice: 100m,
            price: 80m,
            stockQuantity: 10,
            isFeature: false,
            images: new[] { mockFile1.Object, mockFile2.Object },
            productAttributes: new List<ProductAttribute>
                { new ProductAttribute("Color","Red") },
            productOption: new ProductOption
                ("Size",new List<string> { "M", "L" })
        );
        
        // Act
        var result = await validator.TestValidateAsync(command);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public async Task CreateProductValidator_InvalidCommand_ShouldHaveValidationErrors()
    {
        // Arrange
        var (scope, validator) = GetScopedService<CreateProductCommandValidator>();
        var command = new CreateProductCommand(
            sku: "",
            categoryId: 0,
            name: "",
            description: "",
            marketPrice: -50m,
            price: 0m,
            stockQuantity: -10,
            isFeature: false,
            images: Array.Empty<IFormFile>(),
            productAttributes: null,
            productOption: null
        );

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.SKU)
            .WithErrorMessage("SKU is required.");
        result.ShouldHaveValidationErrorFor(c => c.CategoryId)
            .WithErrorMessage("Category ID must be greater than 0.");
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name is required.");
        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage("Description is required.");
        result.ShouldHaveValidationErrorFor(c => c.MarketPrice)
            .WithErrorMessage("Market price must be 0 or greater.");
        result.ShouldHaveValidationErrorFor(c => c.Price)
            .WithErrorMessage("Price must be greater than 0.");
        result.ShouldHaveValidationErrorFor(c => c.StockQuantity)
            .WithErrorMessage("Stock quantity must be 0 or greater.");
        result.ShouldHaveValidationErrorFor(c => c.Images)
            .WithErrorMessage("At least 1 image is required.");
        result.ShouldHaveValidationErrorFor(c => c.ProductAttributes)
            .WithErrorMessage("Product attributes must not be null.");
        result.ShouldHaveValidationErrorFor(c => c.ProductOption)
            .WithErrorMessage("Product options must not be null.");
    }
    
    [Fact]
    public async Task CreateProductValidator_FieldExceedLength_ShouldHaveValidationErrors()
    {
        // Arrange
        var (scope, validator) = GetScopedService<CreateProductCommandValidator>();
        var mockFile = MockFileHelper.CreateMockFormFile("application/pdf");

        var longSku = new string('A', 101);
        var longName = new string('B', 101);
        var longDescription = new string('C', 513);
        var longAttrCode = new string('D', 51);
        var longAttrValue = new string('E', 201);
        var longOptionCode = new string('F', 51);
        var longOptionValue = new string('G', 101);

        var command = new CreateProductCommand(
            sku: longSku,
            categoryId: 1,
            name: longName,
            description: longDescription,
            marketPrice: 100m,
            price: 80m,
            stockQuantity: 10,
            isFeature: false,
            images: new[] { mockFile.Object },
            productAttributes: new List<ProductAttribute>
            {
                new ProductAttribute(longAttrCode, longAttrValue)
            },
            productOption: new ProductOption(longOptionCode, new List<string> { longOptionValue })
        );

        // Act
        var result = await validator.TestValidateAsync(command);

        foreach (var error in result.Errors)
        {
            Console.WriteLine($"Property: {error.PropertyName}, Error: {error.ErrorMessage}");
        }
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.SKU)
            .WithErrorMessage("SKU must not exceed 100 characters.");
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name must not exceed 100 characters.");
        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage("Description must not exceed 512 characters.");
        result.ShouldHaveValidationErrorFor("ProductAttributes[0].Code")
            .WithErrorMessage("Product attribute code must not exceed 50 characters.");
        result.ShouldHaveValidationErrorFor("ProductAttributes[0].Value")
            .WithErrorMessage("Product attribute value must not exceed 200 characters.");
        result.ShouldHaveValidationErrorFor("Images")
            .WithErrorMessage("All uploaded files must be images (e.g., JPEG, PNG).");
        result.ShouldHaveValidationErrorFor(c => c.ProductOption.Code)
            .WithErrorMessage("Product option code must not exceed 50 characters.");
        result.ShouldHaveValidationErrorFor("ProductOption.Values")
            .WithErrorMessage("Each product option value must not be empty and must not exceed 100 characters.");
        result.ShouldNotHaveValidationErrorFor(c => c.CategoryId);
        result.ShouldNotHaveValidationErrorFor(c => c.MarketPrice);
        result.ShouldNotHaveValidationErrorFor(c => c.Price);
        result.ShouldNotHaveValidationErrorFor(c => c.StockQuantity);
    }
    
    [Fact]
    public async Task UpdateProductValidator_ValidCommand_ShouldPass()
    {
        // Arrange
        var (scope, validator) = GetScopedService<UpdateProductCommandValidator>();
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.ContentType).Returns("image/jpeg");

        var command = new UpdateProductCommand(
            id: 1,
            sku: "TEST-SKU-01",
            categoryId: 1,
            name: "Test Product",
            description: "Test Desc",
            marketPrice: 100m,
            price: 80m,
            stockQuantity: 10,
            isFeature: false,
            status: ProductStatus.Available,
            oldImages: new List<string> { "https://127.0.0.1:9090/images/image1.jpg" },
            newImages: new[] { mockFile.Object },
            productAttributes: new List<ProductAttribute> { new ProductAttribute("Color", "Red") },
            productOptions: new ProductOption("Size", new List<string> { "M" })
        );

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public async Task UpdateProductValidator_InvalidCommand_ShouldHaveValidationErrors()
    {
        // Arrange
        var (scope, validator) = GetScopedService<UpdateProductCommandValidator>();
        // var mockFile = new Mock<IFormFile>();
        var mockFile = MockFileHelper.CreateMockFormFile("application/pdf");
        // mockFile.Setup(f => f.ContentType).Returns("application/pdf");

        var command = new UpdateProductCommand(
            id: 0,
            sku: "",
            categoryId: 0,
            name: "",
            description: new string('A', 513),
            marketPrice: -50m,
            price: 0m,
            stockQuantity: -10,
            isFeature: false,
            status: (ProductStatus)999,
            oldImages: new List<string>(),
            newImages: new[] { mockFile.Object },
            productAttributes: null,
            productOptions: null
        );

        // Act
        var result = await validator.TestValidateAsync(command);

        foreach (var error in result.Errors)
        {
            Console.WriteLine($"Property: {error.PropertyName}, Error: {error.ErrorMessage}, Severity: {error.Severity}");
        }
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id)
            .WithErrorMessage("Product ID must be greater than 0.");
        result.ShouldHaveValidationErrorFor(c => c.SKU)
            .WithErrorMessage("SKU is required.");
        result.ShouldHaveValidationErrorFor(c => c.CategoryId)
            .WithErrorMessage("Category ID must be greater than 0.");
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name is required.");
        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage("Description must not exceed 512 characters.");
        result.ShouldHaveValidationErrorFor(c => c.MarketPrice)
            .WithErrorMessage("Market price must be 0 or greater.");
        result.ShouldHaveValidationErrorFor(c => c.Price)
            .WithErrorMessage("Price must be greater than 0.");
        result.ShouldHaveValidationErrorFor(c => c.StockQuantity)
            .WithErrorMessage("Stock quantity must be 0 or greater.");
        result.ShouldHaveValidationErrorFor(c => c.Status)
            .WithErrorMessage("Invalid product status provided.");
        result.ShouldHaveValidationErrorFor("NewImages[0]")
            .WithErrorMessage("All uploaded files must be images (e.g., JPEG, PNG).");
        result.ShouldHaveValidationErrorFor(c => c.OldImages)
            .WithErrorMessage("Old images must contain at least one image.");
        result.ShouldHaveValidationErrorFor(c => c.ProductAttributes)
            .WithErrorMessage("Product must have at least 1 attribute.");
        result.ShouldHaveValidationErrorFor(c => c.ProductOption)
            .WithErrorMessage("Product options must not be null.");
    }
    
    [Fact]
    public async Task DeleteProductValidator_ValidCommand_ShouldPass()
    {
        // Arrange
        var (scope, validator) = GetScopedService<DeleteProductCommandValidator>();
        var command = new DeleteProductCommand(1);

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public async Task DeleteProductValidator_InvalidCommand_ShouldHaveValidationErrors()
    {
        // Arrange
        var (scope, validator) = GetScopedService<DeleteProductCommandValidator>();
        var command = new DeleteProductCommand(0);

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id)
            .WithErrorMessage("Product ID must be greater than 0.");
    }
}

