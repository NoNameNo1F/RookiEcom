using Microsoft.AspNetCore.Authorization;

namespace Microsoft.Extensions.DependencyInjection;

internal static class AuthorizationExtension
{
    internal static IServiceCollection AddAuthorizationExtension(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("roles")
                .Build();
            
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin"));
        });

        return services;
    }
}
