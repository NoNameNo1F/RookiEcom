using RookiEcom.Application.Common;
using RookiEcom.Modules.Product.Contracts.Dtos;

namespace RookiEcom.FrontStore.Abstractions;

public interface IProductService
{
    Task<PagedResult<ProductDto>?> GetProducts(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<PagedResult<ProductDto>?> GetProductsByCategoryId(int categoryId, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default);
    Task<ProductDto?> GetProductById(int productId, CancellationToken cancellationToken = default);
    Task<PagedResult<ProductDto>?> GetFeaturedProducts(
        int pageNumber,
        int pageSize, 
        CancellationToken cancellationToken = default);
}