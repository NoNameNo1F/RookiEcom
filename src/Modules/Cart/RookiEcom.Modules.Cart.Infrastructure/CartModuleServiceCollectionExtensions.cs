using Microsoft.EntityFrameworkCore;
using RookiEcom.Application.Abstraction;
using RookiEcom.Infrastructure.ConfigurationOptions;
using RookiEcom.Modules.Cart.Application.Abstraction;
using RookiEcom.Modules.Cart.Application.Queries;
using RookiEcom.Modules.Cart.Infrastructure.Persistence;

namespace Microsoft.Extensions.DependencyInjection;

public static class CartModuleServiceCollectionExtensions
{
    public static IServiceCollection AddCartModule(this IServiceCollection services, Action<ConnectionStringOptions> configureOptions)
    {
        var settings = new ConnectionStringOptions();
        configureOptions(settings);
        services.Configure(configureOptions);
        
        services.AddCartProcessingPipeline();
        services.AddDbContext<CartContext>(options =>
        {
            options.UseSqlServer(settings.Default, sql =>
                sql.MigrationsAssembly("RookiEcom.WebAPI"));
        });
        
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<CartService>();
        
        return services;
    }
}
