using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RookiEcom.FrontStore.ViewModels;
using RookiEcom.Modules.Product.Contracts.Dtos;

namespace RookiEcom.FrontStore.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IProductRatingService _productRatingService;
    
    public ProductsController(
        IProductService productService, 
        ICategoryService categoryService,
        IProductRatingService productRatingService)
    {
        _productService = productService;
        _categoryService = categoryService;
        _productRatingService = productRatingService;
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
    public async Task<IActionResult> Detail(
        int productId, 
        [FromQuery] int ratingsPage = 1,
        CancellationToken cancellationToken = default)
    {
        var product = await _productService.GetProductById(productId, cancellationToken);
        if (product == null) return NotFound();

        var categoryTreeTask = _categoryService.GetCategoryTree(product.CategoryId, cancellationToken);

        var ratingsTask = _productRatingService
            .GetRatingsByProductIdAsync(productId, ratingsPage, 5, cancellationToken);

        await Task.WhenAll(categoryTreeTask, ratingsTask);
        
        var categoryTree = await categoryTreeTask;
        var productRatings = await ratingsTask;
        
        var viewModel = new ProductDetailViewModel
        {
            Product = product,
            CategoryTree = categoryTree ?? Enumerable.Empty<CategoryDto>(),
            ProductRatings = productRatings
        };

        return View(viewModel);
    }

    [HttpPost("/products/{productId:int}/ratings")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddReview(
        int productId,
        [FromForm] uint score,
        [FromForm] string content,
        [FromForm] IFormFile? image,
        CancellationToken cancellationToken)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = User.FindFirstValue("name") ?? User.Identity.Name ?? "Anonymous";
        
        if (!Guid.TryParse(userIdString, out var customerId))
        {
            TempData["ErrorMessage"] = "You must be logged in to submit a review.";
            return RedirectToAction(nameof(Detail), new { productId });
        }
        
        if (score < 1 || score > 5)
        {
            TempData["ErrorMessage"] = "Rating score must be between 1 and 5.";
            return RedirectToAction(nameof(Detail), new { productId });
        }
        if (string.IsNullOrWhiteSpace(content))
        {
            TempData["ErrorMessage"] = "Review content cannot be empty.";
            return RedirectToAction(nameof(Detail), new { productId });
        }
        if (image != null && image.Length > 2 * 1024 * 1024) // 2MB limit
        {
            TempData["ErrorMessage"] = "Image size cannot exceed 2MB.";
            return RedirectToAction(nameof(Detail), new { productId });
        }
        if (image != null && !image.ContentType.StartsWith("image/"))
        {
            TempData["ErrorMessage"] = "Uploaded file must be an image.";
            return RedirectToAction(nameof(Detail), new { productId });
        }

        var ratingDto = new ProductRatingCreateDto
        {
            CustomerId = customerId,
            CustomerName = userName,
            Score = score,
            Content = content,
            Image = image
        };

        var success = await _productRatingService.CreateRatingAsync(productId, ratingDto, cancellationToken);
        
        if (success)
        {
            TempData["SuccessMessage"] = "Review submitted successfully!";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to submit review. Please try again.";
        }

        return RedirectToAction(nameof(Detail), new { productId });
    }
}