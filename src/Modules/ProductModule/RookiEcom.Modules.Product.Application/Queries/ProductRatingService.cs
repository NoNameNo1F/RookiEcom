using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RookiEcom.Application.Common;
using RookiEcom.Modules.Product.Contracts.Dtos;
using RookiEcom.Modules.Product.Domain.ProductRatingAggregate;

namespace RookiEcom.Modules.Product.Application.Queries;

public class ProductRatingService
{
    private readonly ProductContext _dbContext;
        
    public ProductRatingService(ProductContext dbContext)
    {
        _dbContext = dbContext;
    }

    private static Expression<Func<ProductRating, ProductRatingDto>> ToProductRatingDto()
    {
        return rating => new ProductRatingDto
        {
            Id = rating.Id,
            ProductId = rating.ProductId,
            CustomerId = rating.CustomerId,
            CustomerName = rating.CustomerName,
            Score = rating.Score,
            Content = rating.Content,
            Image = rating.Image,
            CreatedDateTime = rating.CreatedDateTime
        };
    }
    
    public async Task<PagedResult<ProductRatingDto>> GetRatingsPaging(
        int productId,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .AnyAsync(p => p.Id == productId, cancellationToken);
        if (!product)
        {
            return new PagedResult<ProductRatingDto>(new List<ProductRatingDto>(), pageNumber, pageSize, 0);
        }

        var query = _dbContext.ProductRatings
            .AsNoTracking()
            .Where(pr => pr.ProductId == productId);
        var totalCount = await query.CountAsync(cancellationToken);
        var ratings = await query
            .OrderByDescending(r => r.CreatedDateTime)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(ToProductRatingDto())
            .ToListAsync(cancellationToken);

        return new PagedResult<ProductRatingDto>(ratings, pageNumber, pageSize, totalCount);
    }
}