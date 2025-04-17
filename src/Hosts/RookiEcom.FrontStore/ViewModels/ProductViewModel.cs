using RookiEcom.FrontStore.ViewModels.Dtos;
using RookiEcom.FrontStore.ViewModels.ProductDtos;

namespace RookiEcom.FrontStore.ViewModels;

public class ProductViewModel
{
    public ProductDetailDto Product { get; set; }
    public IEnumerable<CategoryDto> CategoryTree { get; set; }
    public Pagination<ProductRatingDto> ProductRatings { get; set; }
}