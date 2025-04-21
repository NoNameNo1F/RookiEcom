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
        
        return services;
    }
}