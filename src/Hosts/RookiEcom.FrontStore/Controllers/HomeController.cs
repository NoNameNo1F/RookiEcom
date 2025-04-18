using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
    private readonly IProductRatingService _productRatingService;
    private readonly ILogger<HomeController> _logger;
    

    public HomeController(
        IProductService productService,
        ICategoryService categoryService,
        IProductRatingService productRatingService,
        ILogger<HomeController> logger)
    {
        _productService = productService;
        _categoryService = categoryService;
        _productRatingService = productRatingService;
        _logger = logger;
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        // var products = await _productService.GetProducts(1, 25, cancellationToken);
        //
        // // await Task.WhenAll(products);
        // return View(new HomeViewModel{Products = products});
        
        return View(new HomeViewModel
        { 
            Products = new Pagination<ProductDto>
            {
                Count = 0,
                Items = new List<ProductDto>(),
                PageNumber = 1,
                PageSize = 25
            },
            Categories = new List<CategoryDto>(),
            ProductFeatures = new Pagination<ProductDto>
            {
                Count = 0,
                Items = new List<ProductDto>(),
                PageNumber = 1,
                PageSize = 25
            }
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}