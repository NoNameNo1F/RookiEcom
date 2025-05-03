using RookiEcom.Modules.Product.Contracts.Dtos;

namespace RookiEcom.FrontStore.ViewModels;

public class HomeViewModel
{
    public Pagination<ProductDto>? ProductFeatures { get; set; }
    public IEnumerable<CategoryDto>? Categories { get; set; }
    public Pagination<ProductDto>? Products { get; set; }
}
