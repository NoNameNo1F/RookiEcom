using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RookiEcom.Application.Common;
using RookiEcom.Application.Storage;
using RookiEcom.Modules.Product.Application.Exceptions;
using RookiEcom.Modules.Product.Contracts.Dtos;
using RookiEcom.Modules.Product.Domain.CategoryAggregate;

namespace RookiEcom.Modules.Product.Application.Queries;

public class CategoryService
{
    private readonly ProductContext _dbContext;
        
    public CategoryService(ProductContext dbContext)
    {
        _dbContext = dbContext;
    }

    private static Expression<Func<Category, CategoryDto>> ToCategoryDto()
    {
        return category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ParentId = category.ParentId,
            IsPrimary = category.IsPrimary,
            Image = category.Image,
            HasChild = category.HasChild,
            CreatedDateTime = category.CreatedDateTime,
            UpdatedDateTime = category.UpdatedDateTime
        };
    }
    public async Task<PagedResult<CategoryDto>> GetAllCategories(int pageNumber, int pageSize,CancellationToken cancellationToken)
    {
        var query = _dbContext.Categories
            .AsNoTracking();

        var categories = await query
            .OrderBy(c => c.UpdatedDateTime)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(ToCategoryDto())
            .ToListAsync(cancellationToken);

        var count = await query.CountAsync(cancellationToken);

        return new PagedResult<CategoryDto>(categories, pageNumber, pageSize, count);
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoryTree(int categoryId, CancellationToken cancellationToken)
    {
        var query = @"
                    WITH CategoryTree AS (
                        SELECT Id, Name, Description, ParentId, IsPrimary, Image, HasChild
                        FROM Categories
                        WHERE Id = @categoryId
                        UNION ALL
                        SELECT c.Id, c.Name, c.Description, c.ParentId, c.IsPrimary, c.Image, c.HasChild
                        FROM Categories c
                        INNER JOIN CategoryTree ct ON c.Id = ct.ParentId
                    )
                    SELECT * FROM CategoryTree
                    ORDER BY CASE WHEN ParentId IS NULL THEN 0 ELSE 1 END";

        return await _dbContext.Categories
            .FromSqlRaw(query, new Microsoft.Data.SqlClient.SqlParameter("@categoryId", categoryId))
            .Select(ToCategoryDto())
            .ToListAsync(cancellationToken);
    }

    public async Task<CategoryDto> GetCategoryById(int categoryId, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories
            .AsNoTracking()
            .Where(c => c.Id == categoryId)
            .Select(ToCategoryDto())
            .FirstOrDefaultAsync(cancellationToken);
        
        if (category == null)
        {
            throw new CategoryNotFoundException(categoryId);
        }

        return category;
    }
}