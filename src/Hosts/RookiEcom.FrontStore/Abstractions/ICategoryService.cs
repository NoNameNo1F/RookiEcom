using RookiEcom.Application.Common;
using RookiEcom.Modules.Product.Contracts.Dtos;

namespace RookiEcom.FrontStore.Abstractions;

public interface ICategoryService
{
    Task<PagedResult<CategoryDto>?> GetAllCategories(int pageNumber = 1, int pageSize = 100, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryDto>?> GetCategoryTree(int categoryId, CancellationToken cancellationToken = default);
    Task<CategoryDto?> GetCategoryById(int categoryId, CancellationToken cancellationToken = default);
}