namespace RookiEcom.WebAPI.Modules.ProductModule.Dtos;

public class CategoryDto
{
    public string Name { get; set; }
    public string Description { get; set; } 
    public int? ParentId { get; set; }
    public bool IsPrimary { get; set; }
    public IFormFile Image { get; set; }
}