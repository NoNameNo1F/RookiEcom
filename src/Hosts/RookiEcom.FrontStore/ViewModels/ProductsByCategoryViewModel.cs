using RookiEcom.Modules.Product.Contracts.Dtos;

namespace RookiEcom.FrontStore.ViewModels;

public class ProductsByCategoryViewModel
{
    public CategoryDto? Category { get; set; }
    public Pagination<ProductDto>? Products { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}