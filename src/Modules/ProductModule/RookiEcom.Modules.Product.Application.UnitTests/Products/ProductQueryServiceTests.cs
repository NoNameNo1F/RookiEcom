using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RookiEcom.Modules.Product.Application.Exceptions;
using RookiEcom.Modules.Product.Application.Queries;
using RookiEcom.Modules.Product.Application.UnitTests.Abstractions;
using RookiEcom.Modules.Product.Application.UnitTests.SeedData;

namespace RookiEcom.Modules.Product.Application.UnitTests.Products;

public class ProductQueryServiceTests : BaseServiceTest
{
    [Theory]
    [InlineData(1,3,10)]
    [InlineData(2,4,10)]
    public async Task GetProducts_ValidPage_ReturnsPagedResult(int pageNumber, int pageSize, int expectedTotalCount)
    {
        // Arrange
        using var scope = ServiceProvider.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<ProductService>();
        
        // Act
        var productResult = await productService.GetProducts(pageNumber, pageSize,default);
        
        // Assert
        Assert.Equal(expectedTotalCount, productResult.PageData.TotalCount);
    }

    [Theory]
    [InlineData(1,5,3)]
    [InlineData(2,4,3)]
    public async Task GetProductsFeature_ValidPage_ReturnsFeaturedProducts(int pageNumber, int pageSize, int expectedTotalCount)
    {
        // Arrange
        using var scope = ServiceProvider.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<ProductService>();
        
        // Act
        var productFeaturedResult = await productService.GetProductsFeature(pageNumber, pageSize, default);
        
        // Assert
        Assert.Equal(expectedTotalCount, productFeaturedResult.PageData.TotalCount);
    }

    [Theory]
    [InlineData(1, "ACR-NITRO5-001")]
    [InlineData(3, "DEL-XPS13-003")]
    [InlineData(5, "DEL-AWX17-005")]
    [InlineData(9, "ACR-ASP5-009")]
    public async Task GetProductById_ProductFound_ReturnsProduct(int productId, string expectedProductSKU)
    {
        // Arrange
        using var scope = ServiceProvider.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<ProductService>();
        
        // Act
        var product = await productService.GetProductById(productId, default);
        
        // Assert
        Assert.Equal(product.Id, productId);
        Assert.Equal(product.SKU, expectedProductSKU);
    }
        
    [Theory]
    [InlineData(15)]
    [InlineData(50)]
    [InlineData(99)]
    public async Task GetProductById_ProductNotFound_ThrowsProductNotFoundException(int productId)
    {
        // Arrange
        using var scope = ServiceProvider.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<ProductService>();
        
        // Act
        Func<Task> action = async () => await productService.GetProductById(productId, default);
        
        // Assert
        await Assert.ThrowsAsync<ProductNotFoundException>(action);
    }

    [Theory]
    [InlineData("ACR-NITRO5-001", "Acer Nitro 5 AN515-58 Gaming Laptop")]
    [InlineData("DEL-XPS13-003", "Dell XPS 13 9310 Laptop")]
    [InlineData("DEL-AWX17-005", "Dell Alienware x17 R2 Gaming Laptop")]
    [InlineData("ACR-ASP5-009", "Acer Aspire 5 A515-56 Slim Laptop")]
    public async Task GetProductBySKU_ProductFound_ReturnsProduct(string productSKU, string expectedProductName)
    {
        // Arrange
        using var scope = ServiceProvider.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<ProductService>();
        
        // Act
        var product = await productService.GetProductBySKU(productSKU, default);
        
        // Assert
        Assert.Equal(product.Name, expectedProductName);
    }
    
    [Theory]
    [InlineData("ACR-NITRO5-00155")]
    [InlineData("")]
    [InlineData("DEL-AWX17-053")]
    [InlineData("ACR-ASP5-0091")]
    public async Task GetProductBySKU_ProductNotFound_ThrowsProductNotFoundException(string productSKU)
    {
        // Arrange
        using var scope = ServiceProvider.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<ProductService>();
        
        // Act
        Func<Task> action = async () => await productService.GetProductBySKU(productSKU, default);
        
        // Assert
        await Assert.ThrowsAsync<ProductNotFoundException>(action);
    }

    [Theory]
    [InlineData(6,5,1,0)]
    [InlineData(1,25,4,10)]
    public async Task GetProductsByCategoryId_ValidCategory_ReturnsPagedProducts(
        int pageNumber,
        int pageSize,
        int categoryId,
        int expectedCurrentCount)
    {
        // Arrange
        using var scope = ServiceProvider.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<ProductService>();
        
        // Act
        var productResult = await productService.GetProductsByCategoryId(pageNumber, pageSize, categoryId, default);
        
        // Assert
        Assert.Equal(expectedCurrentCount,productResult.Items.Count());
    }

    [Theory]
    [InlineData(43)]
    [InlineData(435)]
    [InlineData(7670)]
    public async Task GetProductsByCategoryId_InvalidCategory_ReturnsEmptyResult(int categoryId)
    {
        // Arrange
        using var scope = ServiceProvider.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<ProductService>();
        
        // Act
        var productResult = await productService.GetProductsByCategoryId(1, 5, categoryId, default);
        
        // Assert
        Assert.Equal(0, productResult.Items.Count());
    }
}