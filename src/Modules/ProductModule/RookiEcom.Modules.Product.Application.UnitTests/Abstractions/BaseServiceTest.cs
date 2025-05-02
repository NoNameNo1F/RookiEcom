using Microsoft.Extensions.DependencyInjection;
using RookiEcom.Modules.Product.Application.UnitTests.SeedData;

namespace RookiEcom.Modules.Product.Application.UnitTests.Abstractions;

public class BaseServiceTest : IAsyncLifetime
{
    protected ServiceProvider ServiceProvider { get; private set; }
    
    public virtual async Task InitializeAsync()
    {
        var services = new ProductModuleServiceCollection();
        ServiceProvider = services.BuildServiceProvider();

        var seeder = new SeedProductData(ServiceProvider);
        await seeder.SeedProductModuleData();
    }

    public virtual Task DisposeAsync()
    {
        ServiceProvider?.Dispose();

        return Task.CompletedTask;
    }

    protected (IServiceScope Scope, T Service) GetScopedService<T>() where T : notnull
    {
        var scope = ServiceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<T>();

        return (scope, service);
    }
}