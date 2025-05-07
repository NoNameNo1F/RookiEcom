using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RookiEcom.Application.Common;
using RookiEcom.FrontStore.ViewModels;
using RookiEcom.Modules.Product.Contracts.Dtos;

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
        var productsFeatureTask =  _productService.GetFeaturedProducts(pageNumber, pageSize, cancellationToken);
        var categoriesTask = _categoryService.GetAllCategories(1, 50, cancellationToken);
        
        await Task.WhenAll(productsTask, productsFeatureTask, categoriesTask);
        
        var categoriesPaged = await categoriesTask;
        var productsFeaturePaged = await productsFeatureTask;
        var productsPaged = await productsTask;
        
        var viewModel = new HomeViewModel
        {
            ProductFeatures = productsFeaturePaged,
            Products = productsPaged,
            Categories = categoriesPaged?.Items 
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