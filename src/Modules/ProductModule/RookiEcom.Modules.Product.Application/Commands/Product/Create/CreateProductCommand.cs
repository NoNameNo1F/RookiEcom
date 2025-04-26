using Microsoft.AspNetCore.Http;
using RookiEcom.Application.Contracts;
using RookiEcom.Modules.Product.Domain.ProductAggregate;

namespace RookiEcom.Modules.Product.Application.Commands.Product.Create;

public class CreateProductCommand(
    string sku,
    int categoryId,
    string name,
    string description,
    decimal marketPrice,
    decimal price,
    int stockQuantity,
    bool isFeature,
    IFormFile[] images,
    List<ProductAttribute> productAttributes,
    ProductOption productOption) : CommandBase
{
    public int CategoryId { get; set; } = categoryId;
    public string SKU { get; set; } = sku;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public decimal MarketPrice { get; set; } = marketPrice;
    public decimal Price { get; set; } = price;
    public int StockQuantity { get; set; } = stockQuantity;
    public bool IsFeature { get; set; } = isFeature;
    public IFormFile[] Images { get; set; } = images;
    public List<ProductAttribute> ProductAttributes { get; set; } = productAttributes;
    public ProductOption ProductOption { get; set; } = productOption;
}