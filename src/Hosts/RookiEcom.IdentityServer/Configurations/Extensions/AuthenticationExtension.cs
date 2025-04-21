using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Extensions.DependencyInjection;

public static class AuthenticationExtension
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";

                options.Events.OnValidatePrincipal = async context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<AccountController>>();
                    var requestCookies = context.HttpContext.Request.Cookies;
                    logger.LogInformation("Request cookies during OnValidatePrincipal: {Cookies}",
                        string.Join(", ", requestCookies.Select(c => $"{c.Key}: {c.Value}")));

                    if (context.Principal != null)
                    {
                        logger.LogInformation("OnValidatePrincipal: User authenticated with claims: {Claims}",
                            string.Join(", ", context.Principal.Claims.Select(c => $"{c.Type}: {c.Value}")));
                    }
                    else
                    {
                        logger.LogWarning("OnValidatePrincipal: No authenticated user found.");
                    }

                    await Task.CompletedTask;
                };
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Headers.Authorization;
                        if (string.IsNullOrEmpty(accessToken) == false)
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        
        return services;
    }
}