using Microsoft.AspNetCore.Mvc;
using RookiEcom.FrontStore.ViewModels;
using RookiEcom.Modules.Product.Contracts.Dtos;

namespace RookiEcom.FrontStore.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    
    public ProductsController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 12,
        CancellationToken cancellationToken = default)
    {
        var products = await _productService.GetProducts(pageNumber, pageSize, cancellationToken);

        return View(products);
    }
    [HttpGet("products/cat{categoryId:int}")]
    public async Task<IActionResult> ByCategory(
        int categoryId, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 12, 
        CancellationToken cancellationToken = default)
    {
        var productsTask = _productService.GetProductsByCategoryId(categoryId, page, pageSize, cancellationToken);
        var categoryTask = _categoryService.GetCategoryById(categoryId, cancellationToken);

        await Task.WhenAll(productsTask, categoryTask);
        var products = await productsTask;
        var category = await categoryTask;

        if (category == null) return NotFound();

        var viewModel = new ProductsByCategoryViewModel
        {
            Category = category,
            Products = products,
            CurrentPage = page,
            PageSize = pageSize
        };

        return View(viewModel);
    }
    
    [HttpGet("/products/{productId:int}")]
    public async Task<IActionResult> Detail(int productId, CancellationToken cancellationToken = default)
    {
        var product = await _productService.GetProductById(productId, cancellationToken);
        if (product == null) return NotFound();

        var categoryTree = await _categoryService.GetCategoryTree(product.CategoryId, cancellationToken);
        

        var viewModel = new ProductDetailViewModel
        {
            Product = product,
            CategoryTree = categoryTree ?? Enumerable.Empty<CategoryDto>(),
        };

        return View(viewModel);
    }
}