namespace RookiEcom.FrontStore.ViewModels.ProductDtos;

public class ProductDto
{
    public int Id { get; set; }
    public string SKU { get; set;}
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set;}
    public decimal MarketPrice { get; set;}
    public ProductStatus Status { get; set; }
    public int Sold { get; set; }
    public int StockQuantity { get; set; }
    public bool IsFeature { get; set; }
    public List<string> Images { get; set; }
}

public record ProductAttribute
{
    public string Code { get; set; }
    public string Value { get; set; }

    public ProductAttribute(string code, string value)
    {
        Code = code;
        Value = value;
    }
}

public record ProductOption
{
    public string Code { get; set; }
    public List<string> Values { get; set; }
}