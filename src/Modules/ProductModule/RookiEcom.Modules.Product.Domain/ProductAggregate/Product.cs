using RookiEcom.Domain.SeedWork;

namespace RookiEcom.Modules.Product.Domain.ProductAggregate;

public class Product : IEntity<int>, IAggregateRoot
{
    public int Id { get; set; }
    public string SKU { get; set;}
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set;}
    public decimal MarketPrice { get; set;}
    public decimal Price { get; set; }
    public ProductStatus Status { get; set; }
    public int Sold { get; set; } 
    public int StockQuantity { get; set; }
    public bool IsFeature { get; set; }
    public List<string> Images { get; set; }
    public List<ProductAttribute> ProductAttributes { get; set; }
    public ProductOption ProductOption { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
}