using MediatR;
using FluentValidation;
using RookiEcom.Modules.Product.Application.Queries;
using RookiEcom.Modules.Product.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class ProcessingExtensions
{
    public static IServiceCollection AddProductProcessingPipeline(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assemblies.Application));
        services.AddValidatorsFromAssembly(Assemblies.Application);
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingCommandHandlerBehaviorWithoutResult<,>));
        return services;
    }
}