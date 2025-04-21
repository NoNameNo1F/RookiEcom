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

        services.AddScoped<ProductService>();
        services.AddScoped<CategoryService>();
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkCommandHandlerBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationCommandHandlerBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingCommandHandlerBehaviorWithResult<,>));
        return services;
    }
}
