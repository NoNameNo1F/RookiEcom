using Microsoft.EntityFrameworkCore;
using RookiEcom.Infrastructure.ConfigurationOptions;
using RookiEcom.Modules.Product.Application;
using RookiEcom.Modules.Product.Application.Queries;

namespace Microsoft.Extensions.DependencyInjection;

public static class ProductModuleServiceCollectionExtensions
{
    public static IServiceCollection AddProductModule(this IServiceCollection services, Action<ConnectionStringOptions> configureOptions)
    {
        var settings = new ConnectionStringOptions();
        configureOptions(settings);
        services.Configure(configureOptions);
        
        services.AddProductProcessingPipeline();
        services.AddDbContext<ProductContextImpl>(options =>
        {
            options.UseSqlServer(settings.Default, sql =>
                sql.MigrationsAssembly("RookiEcom.WebAPI"));
        });

        services.AddScoped<ProductContext, ProductContextImpl>();
        services.AddScoped<ProductService>();
        services.AddScoped<CategoryService>();
        return services;
    }
}
