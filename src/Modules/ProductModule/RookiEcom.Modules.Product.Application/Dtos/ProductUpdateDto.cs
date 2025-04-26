using Microsoft.AspNetCore.Http;
using RookiEcom.Modules.Product.Domain.ProductAggregate;

namespace RookiEcom.Modules.Product.Application.Dtos;

public class ProductUpdateDto
{
    public int Id { get; set; }
    public string SKU { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal MarketPrice { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public bool IsFeature { get; set; }
    public ProductStatus Status { get; set; }
    public List<string> OldImages { get; set; }
    public IFormFile[] NewImages { get; set; }
    public List<ProductAttribute> ProductAttributes { get; set; }
    public ProductOption ProductOption { get; set; }
}
