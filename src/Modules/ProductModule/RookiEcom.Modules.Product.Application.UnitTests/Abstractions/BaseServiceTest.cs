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

    public virtual async Task DisposeAsync()
    {
        if (ServiceProvider != null)
        {
            using var scope = ServiceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProductContext>();
            await context.Database.EnsureDeletedAsync();
            ServiceProvider.Dispose();
        }
    }

    protected (IServiceScope Scope, T Service) GetScopedService<T>() where T : notnull
    {
        var scope = ServiceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<T>();

        return (scope, service);
    }
}