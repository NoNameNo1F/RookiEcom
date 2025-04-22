namespace RookiEcom.Modules.Product.Application.Dtos;

public class ProductDto
{
    public int Id { get; set; } 
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set;}
    public decimal MarketPrice { get; set;}
}