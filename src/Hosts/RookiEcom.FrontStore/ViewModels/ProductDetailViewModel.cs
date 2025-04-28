using RookiEcom.FrontStore.ViewModels.Dtos;
using RookiEcom.FrontStore.ViewModels.ProductDtos;

namespace RookiEcom.FrontStore.ViewModels;

public class ProductDetailViewModel
{
    public ProductDetailDto Product { get; set; }
    public IEnumerable<CategoryDto> CategoryTree { get; set; } = Enumerable.Empty<CategoryDto>();
    public Pagination<ProductRatingDto>? ProductRatings { get; set; }
}