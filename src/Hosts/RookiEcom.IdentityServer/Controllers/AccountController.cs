using System.Net;
using System.Security.Claims;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RookiEcom.IdentityServer.Domain;
using RookiEcom.IdentityServer.Models;

namespace RookiEcom.IdentityServer.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IIdentityServerInteractionService _interactionService;

    public AccountController(ILogger<AccountController> logger, UserManager<User> userManager, SignInManager<User> signInManager, IIdentityServerInteractionService interactionService)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _interactionService = interactionService;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
        {
            returnUrl = "/";
        }
        
        return View(
            new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            ModelState.AddModelError("", "Invalid username and password");
            return View(model);
        }

        var claims = await _userManager.GetClaimsAsync(user);
        if (!claims.Any(c => c.Type == "sub"))
        {
            claims.Add(new Claim("sub", user.Id.ToString()));
        }

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
;
        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme,
            ClaimTypes.Name,
            ClaimTypes.Role);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = false,
                RedirectUri = model.ReturnUrl
            });

        _logger.LogInformation($"User {user.FirstName} {user.LastName}" +
            " authenticated with claims: " +
             $"{string.Join(", ", principal.Claims.Select(c => $"{c.Type}: {c.Value}"))}");
        
        if (_interactionService.IsValidReturnUrl(model.ReturnUrl) ||
            Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }
        return Redirect("~/");
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout(string logoutId)
    {
        // await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await _signInManager.SignOutAsync();
        var logoutContext = await _interactionService.GetLogoutContextAsync(logoutId);
        return Redirect(logoutContext?.PostLogoutRedirectUri ?? "/");
    }
}