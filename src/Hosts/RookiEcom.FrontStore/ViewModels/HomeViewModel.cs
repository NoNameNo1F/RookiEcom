using RookiEcom.Application.Common;
using RookiEcom.Modules.Product.Contracts.Dtos;

namespace RookiEcom.FrontStore.ViewModels;

public class HomeViewModel
{
    public PagedResult<ProductDto>? ProductFeatures { get; set; }
    public IEnumerable<CategoryDto>? Categories { get; set; }
    public PagedResult<ProductDto>? Products { get; set; }
}
