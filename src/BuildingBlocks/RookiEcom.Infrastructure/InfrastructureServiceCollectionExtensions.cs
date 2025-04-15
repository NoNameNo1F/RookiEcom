using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RookiEcom.Application.Notification;
using RookiEcom.Application.Storage;
using RookiEcom.Infrastructure.Cache;
using RookiEcom.Infrastructure.Notification.Email;
using RookiEcom.Infrastructure.Notification.Sms;
using RookiEcom.Infrastructure.Storage;

namespace Microsoft.Extensions.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddSingleton<IEmailService, EmailService>();
        // services.AddSingleton<ISmsService, SmsService>();
        
        services.AddSingleton<IBlobService, BlobService>(provider =>
        {
            return new BlobService(configuration["Storage:Account"]!);
        });

        var settings = new DistributedCacheOptions();
        configuration.Bind(settings);

        // services.AddHybridCache(options =>
        // {
        //     options.DefaultEntryOptions = new HybridCacheEntryOptions
        //     {
        //         Expiration = TimeSpan.FromMinutes(5)
        //     };
        // });
        //
        // services.AddStackExchangeRedisCache(opt =>
        // {
        //     opt.Configuration = settings.Redis.Configuration;
        //     opt.InstanceName = settings.Redis.InstanceName;
        // });

        return services;
    }
}