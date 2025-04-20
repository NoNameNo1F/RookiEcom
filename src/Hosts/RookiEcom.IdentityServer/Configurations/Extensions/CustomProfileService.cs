using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;

namespace RookiEcom.IdentityServer.Configurations.Extensions;

public class CustomProfileService : IProfileService
{
    private readonly UserManager<User> _userManager;

    public CustomProfileService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var sub = context.Subject.FindFirst("sub")?.Value;
        if (sub.IsNullOrEmpty())
        {
            return;
        }

        var user = await _userManager.FindByIdAsync(sub);
        if (user == null)
        {
            return;
        }

        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"));
        claims.Add(new Claim("sub", sub));

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        context.IssuedClaims.AddRange(claims);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        context.IsActive = user != null;
    }
}