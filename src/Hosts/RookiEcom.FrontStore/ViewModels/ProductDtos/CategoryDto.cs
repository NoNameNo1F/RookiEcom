﻿namespace RookiEcom.FrontStore.ViewModels.ProductDtos;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int? ParentId { get; set; }
    public bool IsPrimary { get; set; }
    public string Image { get; set; }
    public bool HasChild { get; set; }
}