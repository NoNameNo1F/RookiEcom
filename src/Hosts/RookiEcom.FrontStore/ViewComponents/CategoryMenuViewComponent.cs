using Microsoft.AspNetCore.Mvc;
using RookiEcom.Modules.Product.Contracts.Dtos;

namespace RookiEcom.FrontStore.ViewComponents;

public class CategoryMenuViewComponent : ViewComponent
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoryMenuViewComponent> _logger;

    public CategoryMenuViewComponent(ICategoryService categoryService, ILogger<CategoryMenuViewComponent> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await _categoryService.GetAllCategories(1, 100);
        if (categories == null)
        {
            _logger.LogWarning("Failed to retrieve categories for the menu.");
            return View(Enumerable.Empty<CategoryDto>());
        }

        return View(categories.Items);
    }
}