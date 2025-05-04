using Microsoft.AspNetCore.Http;

namespace RookiEcom.Modules.Product.Contracts.Dtos;

public class CategoryBodyDto
{
    public string Name { get; set; }
    public string Description { get; set; } 
    public int? ParentId { get; set; }
    public bool IsPrimary { get; set; }
    public IFormFile? Image { get; set; }
}