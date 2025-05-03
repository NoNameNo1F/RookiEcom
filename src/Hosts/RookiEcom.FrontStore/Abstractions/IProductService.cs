using RookiEcom.Modules.Product.Contracts.Dtos;

namespace RookiEcom.FrontStore.Abstractions;

public interface IProductService
{
    Task<Pagination<ProductDto>?> GetProducts(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<Pagination<ProductDto>?> GetProductsByCategoryId(int categoryId, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default);
    Task<ProductDto?> GetProductById(int productId, CancellationToken cancellationToken = default);
    Task<Pagination<ProductDto>?> GetFeaturedProducts(
        int pageNumber,
        int pageSize, 
        CancellationToken cancellationToken = default);
}