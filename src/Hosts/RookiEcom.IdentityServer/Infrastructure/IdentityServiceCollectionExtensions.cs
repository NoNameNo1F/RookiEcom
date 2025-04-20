using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class IdentityServiceCollectionExtensions
{
    public static IServiceCollection AddDuendeIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityContext>(options =>
            options.UseSqlServer(configuration["ConnectionStrings:Identity"],
                sql => sql.MigrationsAssembly(typeof(Program).Assembly.FullName)));

        services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

        services
            .AddIdentityServer(options =>
                {
                    options.KeyManagement.Enabled = false;
                    options.Authentication.CookieAuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.Authentication.CookieLifetime = TimeSpan.FromMinutes(30);
                    options.Authentication.CookieSlidingExpiration = true;
                }
            )
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseSqlServer(configuration["ConnectionStrings:Identity"],
                        sql => sql.MigrationsAssembly(typeof(Program).Assembly.FullName));
            }) 
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseSqlServer(configuration["ConnectionStrings:Identity"],
                        sql => sql.MigrationsAssembly(typeof(Program).Assembly.FullName));
                options.EnableTokenCleanup = true;
            })
            .AddAspNetIdentity<User>()
            .AddProfileService<CustomProfileService>()
            .AddDeveloperSigningCredential();
        
        return services;
    }

    public static IServiceCollection AddIdentityServiceCollection(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<UserQueryService>();
        
        return services;
    }
}