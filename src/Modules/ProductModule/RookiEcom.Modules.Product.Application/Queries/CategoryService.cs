using Microsoft.EntityFrameworkCore;
using RookiEcom.Application.Common;
using RookiEcom.Application.Storage;
using RookiEcom.Modules.Product.Domain.CategoryAggregate;

namespace RookiEcom.Modules.Product.Application.Queries;

public class CategoryService
{
    private readonly ProductContext _dbContext;
    private readonly IBlobService _blobService;
        
    public CategoryService(
        ProductContext dbContext,
        IBlobService blobService)
    {
        _dbContext = dbContext;
        _blobService = blobService;
    }

    public async Task<int> CreateCategory(
        string name,
        string description,
        int? parentId,
        IFormFile image, 
        CancellationToken cancellationToken)
    {
        return 1;
    }
    
    public async Task<IEnumerable<Category>> GetAllCategories(CancellationToken cancellationToken)
    {
        return await _dbContext.Categories
            .Where(c => c.IsPrimary == true)
            .ToListAsync(cancellationToken);
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
}