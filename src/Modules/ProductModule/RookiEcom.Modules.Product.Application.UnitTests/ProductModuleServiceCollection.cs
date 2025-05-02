using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using RookiEcom.Modules.Product.Application.Queries;
using RookiEcom.Modules.Product.Infrastructure.Persistence;

namespace RookiEcom.Modules.Product.Application.UnitTests;

public class ProductModuleServiceCollection : ServiceCollection
{
    private readonly Guid _id;

    public ProductModuleServiceCollection()
    {
        _id = Guid.NewGuid();
        
        this.AddSingleton<ILoggerFactory>(new LoggerFactory());
        this.AddDbContext<ProductContextImpl>(options =>
            options
                .UseInMemoryDatabase(_id.ToString())
                .UseLoggerFactory(this.BuildServiceProvider().GetRequiredService<ILoggerFactory>())
                .EnableSensitiveDataLogging()
            );
        this.AddScoped<ProductContext, ProductContextImpl>();

        this.AddScoped<Mock<ProductService>>(provider =>
        {
            var productContext = provider.GetRequiredService<ProductContext>();
            return new Mock<ProductService>(productContext);
        });
        this.AddScoped<ProductService>(provider => provider.GetRequiredService<Mock<ProductService>>().Object);
        
        this.AddScoped<Mock<CategoryService>>(provider =>
        {
            var productContext = provider.GetRequiredService<ProductContext>();
            return new Mock<CategoryService>(productContext);
        });
        this.AddScoped<CategoryService>(provider => provider.GetRequiredService<Mock<CategoryService>>().Object);
    }
}