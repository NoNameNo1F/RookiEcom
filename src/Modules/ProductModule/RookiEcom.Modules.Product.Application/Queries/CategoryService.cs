using Microsoft.EntityFrameworkCore;
using RookiEcom.Application.Common;
using RookiEcom.Application.Storage;
using RookiEcom.Modules.Product.Application.Exceptions;
using RookiEcom.Modules.Product.Domain.CategoryAggregate;

namespace RookiEcom.Modules.Product.Application.Queries;

public class CategoryService
{
    private readonly ProductContext _dbContext;
        
    public CategoryService(ProductContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<PagedResult<Category>> GetAllCategories(int pageSize, int pageNumber,CancellationToken cancellationToken)
    {
        var query = _dbContext.Categories
            .AsNoTracking()
            .Where(c => c.IsPrimary == true);

        var categories = await query
            .OrderBy(c => c.UpdatedDateTime)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var count = await query.CountAsync(cancellationToken);

        return new PagedResult<Category>(categories, pageNumber, pageSize, count);
    }

    public async Task<IEnumerable<Category>> GetCategoryTree(int categoryId, CancellationToken cancellationToken)
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
            .ToListAsync(cancellationToken);
    }

    public async Task<Category> GetCategoryById(int categoryId, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);
        
        if (category == null)
        {
            throw new CategoryNotFoundException(categoryId);
        }

        return category;
    }
}