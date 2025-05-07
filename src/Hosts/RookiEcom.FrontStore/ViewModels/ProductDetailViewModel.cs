using RookiEcom.Application.Common;
using RookiEcom.Modules.Product.Contracts.Dtos;

namespace RookiEcom.FrontStore.ViewModels;

public class ProductDetailViewModel
{
    public ProductDto Product { get; set; }
    public IEnumerable<CategoryDto> CategoryTree { get; set; } = Enumerable.Empty<CategoryDto>();
    public PagedResult<ProductRatingDto>? ProductRatings { get; set; }
}