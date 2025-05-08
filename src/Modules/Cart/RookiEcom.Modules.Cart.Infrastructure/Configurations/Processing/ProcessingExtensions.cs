using MediatR;
using FluentValidation;
using RookiEcom.Modules.Cart.Infrastructure;
using RookiEcom.Modules.Cart.Infrastructure.Configurations.Processing;

namespace Microsoft.Extensions.DependencyInjection;

public static class ProcessingExtensions
{
    public static IServiceCollection AddCartProcessingPipeline(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assemblies.Application));
        services.AddValidatorsFromAssembly(Assemblies.Application);
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingCommandHandlerBehaviorWithoutResult<,>));
        
        return services;
    }
}