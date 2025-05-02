using Microsoft.Extensions.DependencyInjection;
using RookiEcom.Modules.Product.Application.Exceptions;
using RookiEcom.Modules.Product.Application.Queries;
using RookiEcom.Modules.Product.Application.UnitTests.Abstractions;
using RookiEcom.Modules.Product.Application.UnitTests.SeedData;

namespace RookiEcom.Modules.Product.Application.UnitTests.Categories;

public class CategoryServiceTests : BaseServiceTest
{
    [Fact]
    public async Task GetCategories_ValidPage_ReturnsPagedResult()
    {
        // Arrange
        using var scope = ServiceProvider.CreateScope();
        var categoryService = scope.ServiceProvider.GetRequiredService<CategoryService>();
        
        // Act
        var categories = await categoryService.GetAllCategories(1, 3, default);
        
        // Assert
        Assert.Equal(4, categories.PageData.TotalCount);
    }
    
    [Theory]
    [InlineData(1, "Electronic")]
    [InlineData(2, "Computer")]
    [InlineData(3, "Mobile")]
    [InlineData(4, "Laptop")]
    public async Task GetCategoryById_CategoryFound_ReturnsCategory(int categoryId, string expectedCategoryName)
    {
        // Arrange
        using var scope = ServiceProvider.CreateScope();
        var categoryService = scope.ServiceProvider.GetRequiredService<CategoryService>();
        
        // Act
        var categoryDto = await categoryService.GetCategoryById(categoryId, default);

        // Assert
        Assert.Equal(categoryDto.Id, categoryId);
        Assert.Equal(categoryDto.Name, expectedCategoryName);
    }

    [Theory]
    [InlineData(14)]
    [InlineData(20)]
    [InlineData(99)]
    public async Task GetCategoryById_CategoryNotFound_ThrowsCategoryNotFoundException(int categoryId)
    {
        // Arrange
        using var scope = ServiceProvider.CreateScope();
        var categoryService = scope.ServiceProvider.GetRequiredService<CategoryService>();
        
        // Act
        Func<Task> action = async () => await categoryService.GetCategoryById(categoryId, default);

        // Assert
        await Assert.ThrowsAsync<CategoryNotFoundException>(action);
    }
}