using RookiEcom.FrontStore.ViewModels.ProductDtos;

namespace RookiEcom.FrontStore.Abstractions;

public interface IProductService
{
    Task<Pagination<ProductDto>> GetProducts(int pageNumber, int pageSize, CancellationToken cancellationToken);
}