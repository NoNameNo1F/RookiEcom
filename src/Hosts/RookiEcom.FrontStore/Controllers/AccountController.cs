using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RookiEcom.FrontStore.ViewModels;
using RookiEcom.FrontStore.ViewModels.UserDtos;

namespace RookiEcom.FrontStore.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IUserService userService, ILogger<AccountController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("/account/profile")]
    [Authorize]
    public async Task<IActionResult> Profile(CancellationToken cancellationToken)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var userId))
        {
            _logger.LogWarning("Could not parse user ID from claims for user: {Username}", User.Identity?.Name);
            return RedirectToAction(nameof(Login), new { ReturnUrl = Request.Path });
        }

        UserDto? userProfile = null;
        try
        {
            userProfile = await _userService.GetUserProfileAsync(userId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching user profile for user ID: {UserId}", userId);
            return View("Error");
        }


        if (userProfile == null)
        {
            _logger.LogWarning("User profile not found for user ID: {UserId}", userId);
            ViewBag.ErrorMessage = "Your profile could not be found.";
            return View("Error");
        }

        var viewModel = new UserProfileViewModel { UserProfile = userProfile };
        return View(viewModel);
    }

    public IActionResult Login(string returnUrl = "/")
    {
        return Challenge(
            new AuthenticationProperties { RedirectUri = returnUrl },
            OpenIdConnectDefaults.AuthenticationScheme);
    }
    
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return SignOut(new AuthenticationProperties { RedirectUri = "/" },
            OpenIdConnectDefaults.AuthenticationScheme);
    }
}