using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using RookiEcom.Application.Storage;

namespace Microsoft.Extensions.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IBlobService, BlobService>(provider =>
        {
            return new BlobService(configuration["Storage:Account"]!);
        });

        var settings = new DistributedCacheOptions();
        configuration.Bind(settings);

        services.AddHybridCache(options =>
        {
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(5)
            };
        });
        
        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = settings.Redis.Configuration;
            opt.InstanceName = settings.Redis.InstanceName;
        });
        
        return services;
    }
}