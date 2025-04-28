using System.Security.Claims;
using Asp.Versioning;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


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
    [ValidateAntiForgeryToken]
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
    public IActionResult Register(string returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
        {
            returnUrl = "/";
        }

        return View(new RegisterViewModel { ReturnUrl = returnUrl });
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        if (model.RePassword != model.Password)
        {
            ModelState.AddModelError(string.Empty, "Password should be match with RePassword.");
            return View(model);
        }
        
        var user = new User
        {
            UserName = model.UserName,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            DoB = model.DoB
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
        
        await _userManager.AddToRoleAsync(user, "Customer");
        var claims = new List<Claim>
        {
            new Claim("sub", user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{model.FirstName} {model.LastName}"),
            new Claim(ClaimTypes.Role, "Customer")
        };
        await _userManager.AddClaimsAsync(user, claims);
        _logger.LogInformation($"User {user.FirstName} {user.LastName} registered.");

        if (_interactionService.IsValidReturnUrl(model.ReturnUrl) || Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }
        return Redirect("~/");
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout(string logoutId)
    {
        await _signInManager.SignOutAsync();
        Response.Cookies.Append($"idsrv.session", "", new CookieOptions
        {
            Expires = DateTime.Now.AddDays(-1),
            Path = "/",
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        Response.Cookies.Append(".AspNetCore.Cookies", "", new CookieOptions
        {
            Expires = DateTime.Now.AddDays(-1),
            Path = "/",
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        var logoutContext = await _interactionService.GetLogoutContextAsync(logoutId);
        return Redirect(logoutContext?.PostLogoutRedirectUri!);
    }
}