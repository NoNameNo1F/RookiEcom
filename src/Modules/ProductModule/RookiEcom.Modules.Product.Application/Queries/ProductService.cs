using System.Collections.Immutable;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RookiEcom.Application.Common;
using RookiEcom.Modules.Product.Application.Exceptions;
using RookiEcom.Modules.Product.Contracts.Dtos;
using RookiEcom.Modules.Product.Domain.Shared;

namespace RookiEcom.Modules.Product.Application.Queries;

public class ProductService
{
    private readonly ProductContext _dbContext;

    public ProductService(ProductContext dbContext)
    {
        _dbContext = dbContext;
    }

    private static Expression<Func<Domain.ProductAggregate.Product, ProductDto>> ToProductDto()
    {
        return product => new ProductDto
        {
            Id = product.Id,
            SKU = product.SKU,
            CategoryId = product.CategoryId,
            Name = product.Name,
            Description = product.Description,
            MarketPrice = product.MarketPrice,
            Price = product.Price,
            Status = product.Status,
            Sold = product.Sold,
            StockQuantity = product.StockQuantity,
            IsFeature = product.IsFeature,
            // Images = new List<string>(), // Default empty list
            // ProductAttributes = null, // Default null
            // ProductOption = null,
            Images = product.Images.ToList(),
            ProductAttributes = product.ProductAttributes
                .Select(pa => new ProductAttribute
                {
                    Code = pa.Code,
                    Value = pa.Value
                }).ToList(),
            ProductOption = product.ProductOption == null ? 
                null : new ProductOption
                {
                    Code = product.ProductOption.Code,
                    Values = product.ProductOption.Values.ToList()
                },
            CreatedDateTime = product.CreatedDateTime,
            UpdatedDateTime = product.UpdatedDateTime
        };
    }
    
    public async Task<PagedResult<ProductDto>> GetProducts(int pageNumber, int pageSize,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Products
            .AsNoTracking();
        
        var products = await query
            .OrderBy(p => p.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(ToProductDto())
            .ToListAsync(cancellationToken);
        
        var count = await query.CountAsync(cancellationToken);
        return new PagedResult<ProductDto>(products, pageNumber, pageSize, count);
    }
    
    public async Task<PagedResult<ProductDto>> GetProductsFeature(int pageNumber, int pageSize,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Products
            .AsNoTracking()
            .Where(p => p.IsFeature == true);
        
        var products = await query
            .OrderBy(p => p.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(ToProductDto())
            .ToListAsync(cancellationToken);
        
        var count = await query.CountAsync(cancellationToken);
        return new PagedResult<ProductDto>(products, pageNumber, pageSize, count);
    }
    
    public async Task<PagedResult<ProductDto>> GetProductsByCategoryId(int pageNumber, int pageSize, int categoryId,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Products
            .AsNoTracking()
            .Where(p => p.CategoryId == categoryId);
        
        var products = await query
            .OrderBy(p => p.UpdatedDateTime)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(ToProductDto())
            .ToListAsync(cancellationToken);
        
        var count = await query.CountAsync(cancellationToken);

        return new PagedResult<ProductDto>(products, pageNumber, pageSize, count);
    }
    
    public async Task<ProductDto> GetProductById(int productId,
        CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .Where(p => p.Id == productId)
            .Select(ToProductDto())
            .FirstOrDefaultAsync(cancellationToken);
        
        if (product == null)
        {
            throw new ProductNotFoundException(productId);
        }

        return product;
    }
    
    public async Task<ProductDto> GetProductBySKU(string productSKU,
        CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .Where(p => p.SKU == productSKU)
            .Select(ToProductDto())
            .FirstOrDefaultAsync(cancellationToken);

        if (product == null)
        {
            throw new ProductNotFoundException(productSKU);
        }

        return product;
    }
}


