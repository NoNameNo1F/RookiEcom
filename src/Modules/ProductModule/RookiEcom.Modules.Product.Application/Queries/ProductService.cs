using Microsoft.EntityFrameworkCore;
using RookiEcom.Application.Common;
using RookiEcom.Modules.Product.Application.Exceptions;

namespace RookiEcom.Modules.Product.Application.Queries;

public class ProductService
{
    private readonly ProductContext _dbContext;

    public ProductService(ProductContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<Domain.ProductAggregate.Product>> GetProducts(int pageNumber, int pageSize,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Products
            .AsNoTracking()
            .Where(p => p.IsFeature == true);
        
        var products = await query
            .OrderBy(p => p.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        var count = await query.CountAsync(cancellationToken);
        return new PagedResult<Domain.ProductAggregate.Product>(products, pageNumber, pageSize, count);
    }

    public async Task<PagedResult<Domain.ProductAggregate.Product>> GetProductByCategoryId(int pageNumber, int pageSize, int categoryId,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Products
            .AsNoTracking()
            .Where(p => p.CategoryId == categoryId);
        
        var products = await query
            .OrderBy(p => p.UpdatedDateTime)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        var count = await query.CountAsync(cancellationToken);

        return new PagedResult<Domain.ProductAggregate.Product>(products, pageNumber, pageSize, count);
    }
    
    public async Task<Domain.ProductAggregate.Product> GetProductById(int productId,
        CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .Where(p => p.Id == productId)
            .FirstOrDefaultAsync(cancellationToken);

        if (product == null)
        {
            throw new ProductNotFoundException(productId);
        }

        return product;
    }
    
    public async Task<Domain.ProductAggregate.Product> GetProductBySKU(string productSKU,
        CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .Where(p => p.SKU == productSKU)
            .FirstOrDefaultAsync(cancellationToken);

        if (product == null)
        {
            throw new ProductNotFoundException(productSKU);
        }

        return product;
    }
}


