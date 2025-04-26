using Microsoft.AspNetCore.Http;
using RookiEcom.Application.Contracts;

namespace RookiEcom.Modules.Product.Application.Commands.Category.Update;

public sealed class UpdateCategoryCommand(
    int id,
    string name, 
    string description, 
    int? parentId,
    bool isPrimary,
    IFormFile image) : CommandBase
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public int? ParentId { get; set; } = parentId ?? 0; 
    public bool IsPrimary { get; set; } = isPrimary;
    public IFormFile? Image { get; set; } = image;
}