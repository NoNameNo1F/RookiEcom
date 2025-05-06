using RookiEcom.Modules.Product.Application.Queries;
using RookiEcom.Modules.Product.Application.UnitTests.Abstractions;

namespace RookiEcom.Modules.Product.Application.UnitTests.ProductRatings;

public class ProductRatingQueryServiceTests : BaseServiceTest
{
    [Theory]
    [InlineData(1,1,3,3)]
    [InlineData(2, 2,4,2)]
    public async Task GetProductRatings_ValidPageAndProductId_ReturnsPagedResult(
        int productId, 
        int pageNumber, 
        int pageSize, 
        int expectedTotalCount)
    {
        // Arrange
        var (scope, productRatingService) = GetScopedService<ProductRatingService>();
        
        // Act
        var ratingResult = await productRatingService
            .GetRatingsPaging(productId,pageNumber, pageSize,default);
        
        // Assert
        Assert.Equal(expectedTotalCount, ratingResult.PageData.TotalCount);
    }

    [Theory]
    [InlineData(99, 1, 3, 0)]
    [InlineData(87, 1, 3, 0)]
    public async Task GetProductRatings_ProductIdNotFound_ReturnsEmptyPagedResult(
        int productId,
        int pageNumber,
        int pageSize,
        int expectedTotalCount)
    {
        // Arrange
        var (scope, productRatingService) = GetScopedService<ProductRatingService>();
        
        // Act
        var ratingResult = await productRatingService
            .GetRatingsPaging(productId, pageNumber, pageSize, default);
        
        // Assert
        Assert.Equal(expectedTotalCount, ratingResult.PageData.TotalCount);
    }
}