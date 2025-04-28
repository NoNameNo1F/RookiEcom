using RookiEcom.FrontStore.ViewModels.ProductDtos;

namespace RookiEcom.FrontStore.ViewModels;

public class HomeViewModel
{
    public Pagination<ProductDto>? ProductFeatures { get; set; }
    public IEnumerable<CategoryDto>? Categories { get; set; }
    public Pagination<ProductDto>? Products { get; set; }
}
