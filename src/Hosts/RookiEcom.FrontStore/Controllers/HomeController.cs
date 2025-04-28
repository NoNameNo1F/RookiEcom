using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using RookiEcom.FrontStore.ViewModels;
using RookiEcom.FrontStore.ViewModels.ProductDtos;

namespace RookiEcom.FrontStore.Controllers;

[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(
        IProductService productService,
        ICategoryService categoryService,
        ILogger<HomeController> logger)
    {
        _productService = productService;
        _categoryService = categoryService;
        _logger = logger;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 25,
        CancellationToken cancellationToken = default)
    {
        var productsTask =  _productService.GetProducts(pageNumber, pageSize, cancellationToken);
        var categoriesTask = _categoryService.GetAllCategories(1, 50, cancellationToken);
        
        await Task.WhenAll(productsTask, categoriesTask);
        
        var categories = await categoriesTask;
        var products = await productsTask;
        var productsFeature = products?.Items.Where(p => p.IsFeature).Take(5);
        var latest = products?.Items.OrderByDescending(p => p.Id).Take(10);
    
        
        var viewModel = new HomeViewModel
        {
            Products = latest != null
                ? new Pagination<ProductDto>
                {
                    Items = latest,
                    Count = latest.Count()
                }
                : null,
            ProductFeatures = productsFeature != null
                ? new Pagination<ProductDto>
                {
                    Count = productsFeature.Count(),
                    Items = productsFeature
                }
                : null,
            Categories = categories?.Items ?? Enumerable.Empty<CategoryDto>()
        };
        return View(viewModel);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    public IActionResult About()
    {
        return View();
    }
}