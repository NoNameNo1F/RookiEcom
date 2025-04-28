using RookiEcom.FrontStore.ViewModels.ProductDtos;

namespace RookiEcom.FrontStore.Abstractions;

public interface ICategoryService
{
    Task<Pagination<CategoryDto>?> GetAllCategories(int pageNumber = 1, int pageSize = 100, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryDto>?> GetCategoryTree(int categoryId, CancellationToken cancellationToken = default);
    Task<CategoryDto?> GetCategoryById(int categoryId, CancellationToken cancellationToken = default);
}