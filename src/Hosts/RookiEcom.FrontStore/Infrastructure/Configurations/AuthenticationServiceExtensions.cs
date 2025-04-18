using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace RookiEcom.FrontStore.Infrastructure.Configurations;

public static class AuthenticationServiceExtensions
{
    public static IServiceCollection AddAuthenticationIdP(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie(options =>
        {
            options.Cookie.Name = "frontstore";
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        })
        .AddOpenIdConnect(options =>
        {
            options.Authority = "https://localhost:8080";
            options.ClientId = "rookiecom-frontstore";
            options.ClientSecret = "Rookie";
            options.ResponseType = "code";
            options.SaveTokens = true;
            options.Scope.Add(OpenIdConnectScope.OpenId);
            options.Scope.Add("profile");
            options.Scope.Add("rookiecom-webapi");
            options.Scope.Add("role");
            options.GetClaimsFromUserInfoEndpoint = true;
            options.TokenValidationParameters.NameClaimType = ClaimTypes.Name;
            options.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;

            options.Events = new OpenIdConnectEvents
            {
                OnRemoteFailure = context =>
                {
                    context.Response.Redirect("/Home/Error?message=" +
                                              $"{{Uri.EscapeDataString(context.Failure?.Message ?? \"Authentication failed\")}}");
                    context.HandleResponse();
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    context.Response.Redirect($"/Home/Error?message={Uri.EscapeDataString(context.Exception.Message)}");
                    context.HandleResponse();
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine("Token validated: " + context.SecurityToken.RawData);
                    return Task.CompletedTask;
                }
            };
        });
        return services;
    }
}