using Microsoft.AspNetCore.Http;
using RookiEcom.Application.Contracts;

namespace RookiEcom.Modules.Product.Application.Commands.Category.Create;

public sealed class CreateCategoryCommand(
    string name, 
    string description, 
    int? parentId,
    bool isPrimary,
    IFormFile image) : CommandBase
{
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public int? ParentId { get; set; } = parentId;
    public bool IsPrimary { get; set; } = isPrimary;
    public IFormFile Image { get; set; } = image;
}