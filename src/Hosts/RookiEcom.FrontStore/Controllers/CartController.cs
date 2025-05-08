using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RookiEcom.FrontStore.ViewModels;
using RookiEcom.Modules.Cart.Contracts.Dtos;
using RookiEcom.Modules.Product.Contracts.Dtos;

namespace RookiEcom.FrontStore.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IProductService _productService;
    private readonly ILogger<CartController> _logger;

    public CartController(ICartService cartService, IProductService productService, ILogger<CartController> logger)
    {
        _cartService = cartService;
        _productService = productService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var cart = await _cartService.GetCartAsync(cancellationToken);
        if (cart == null)
        {
            _logger.LogWarning("Cart could not be retrieved for user {UserId}. Displaying empty cart.", User.Identity?.Name);
            cart = new CartDto { CartItems = new List<CartItemDto>() };
        }

        var viewModel = new CartViewModel
        {
            Cart = cart 
        };
        
        return View(viewModel);
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCartSummaryJson(CancellationToken cancellationToken)
    {
        CartDto? cart = null;
        int itemCount = 0;

        if (User.Identity?.IsAuthenticated ?? false)
        {
            try {
                cart = await _cartService.GetCartAsync(cancellationToken);
                itemCount = cart?.CartItems?.Count ?? 0;
            } catch (Exception ex) {
                _logger.LogError(ex, "Error fetching cart summary for header.");
                return Json(new { success = false, count = 0, message = "Error fetching cart count." });
            }
        }

        return Json(new { success = true, count = itemCount });
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddItem(int productId, int quantity = 1, CancellationToken cancellationToken = default)
    {
        if (quantity <= 0) quantity = 1;
        
        ProductDto? product = null;
        try
        {
            product = await _productService.GetProductById(productId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AJAX: Error fetching product details for ProductId {ProductId}.", productId);
            return Json(new { success = false, message = "Could not retrieve product details." });
        }

        if (product == null)
        {
            return Json(new { success = false, message = "Product not found." });
        }

        var addItemDto = new AddCartItemDto
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Price = product.Price,
            Quantity = quantity,
            Image = product.Images?.FirstOrDefault()
        };

        var success = await _cartService.AddItemToCartAsync(addItemDto, cancellationToken);
        if (success)
        {
            return Json(new { success = true, message = $"{product.Name} (x{quantity}) added to cart." });
        }
        else
        {
            return Json(new { success = false, message = $"Failed to add {product.Name} to cart." });
        }
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateQuantity([FromForm] int cartItemId,[FromForm] int quantity, CancellationToken cancellationToken)
    {
        if (quantity < 0)
        {
            TempData["ErrorMessage"] = "Quantity cannot be negative.";
            return RedirectToAction(nameof(Index));
        }

        var success = await _cartService.UpdateCartItemAsync(cartItemId, quantity, cancellationToken);
        if (!success)
        {
            TempData["ErrorMessage"] = "Failed to update cart quantity. Please try again.";
        }

        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveItem(int cartItemId, CancellationToken cancellationToken)
    {
        var success = await _cartService.RemoveItemFromCartAsync(cartItemId, cancellationToken);
        if (success)
        {
            TempData["SuccessMessage"] = "Item removed from cart.";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to remove item from cart. Please try again.";
        }
        
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Clear(CancellationToken cancellationToken)
    {
        var success = await _cartService.ClearCartAsync(cancellationToken);
        if (success)
        {
            TempData["SuccessMessage"] = "Cart cleared.";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to clear cart. Please try again.";
        }
        
        return RedirectToAction(nameof(Index));
    }
}