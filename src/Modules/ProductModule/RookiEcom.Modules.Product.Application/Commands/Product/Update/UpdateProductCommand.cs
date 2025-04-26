using Microsoft.AspNetCore.Http;
using RookiEcom.Application.Contracts;
using RookiEcom.Modules.Product.Domain.ProductAggregate;

namespace RookiEcom.Modules.Product.Application.Commands.Product.Update;

public sealed class UpdateProductCommand(
    int id,
    string sku,
    int categoryId,
    string name,
    string description,
    decimal marketPrice,
    decimal price,
    int stockQuantity,
    bool isFeature,
    List<string> oldImages,
    IFormFile[] newImages,
    List<ProductAttribute> productAttributes,
    ProductOption productOptions) : CommandBase
{
    public int Id { get; set; } = id;
    public int CategoryId { get; set; } = categoryId;
    public string SKU { get; set; } = sku;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public decimal MarketPrice { get; set; } = marketPrice;
    public decimal Price { get; set; } = price;
    public int StockQuantity { get; set; } = stockQuantity;
    public bool IsFeature { get; set; } = isFeature;
    public List<string> OldImages { get; set; } = oldImages;
    public IFormFile[] NewImages { get; set; } = newImages;
    public List<ProductAttribute> ProductAttributes { get; set; } = productAttributes;
    public ProductOption ProductOption { get; set; } = productOptions;
}