using RookiEcom.Modules.Product.Domain.ProductAggregate;
using IFormFile = RookiEcom.Application.Common.IFormFile;

namespace RookiEcom.WebAPI.Modules.ProductModule.Dtos;

public class ProductDto
{
    public string SKU { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal MarketPrice { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public bool IsFeature { get; set; }
    public IFormFile[] Images { get; set; }
    public List<ProductAttribute> ProductAttributes { get; set; }
    public ProductOption ProductOption { get; set; }
}