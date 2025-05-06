using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using RookiEcom.Application.Storage;
using RookiEcom.Modules.Product.Application.Queries;
using RookiEcom.Modules.Product.Infrastructure;
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
        this.AddScoped<ProductService>(provider => 
            provider.GetRequiredService<Mock<ProductService>>().Object);
        
        this.AddScoped<Mock<CategoryService>>(provider =>
        {
            var productContext = provider.GetRequiredService<ProductContext>();
            return new Mock<CategoryService>(productContext);
        });
        this.AddScoped<CategoryService>(provider => 
            provider.GetRequiredService<Mock<CategoryService>>().Object);

        this.AddScoped<Mock<ProductRatingService>>(provider =>
        {
            var productContext = provider.GetRequiredService<ProductContext>();
            return new Mock<ProductRatingService>(productContext);
        });
        this.AddScoped<ProductRatingService>(provider =>
            provider.GetRequiredService<Mock<ProductRatingService>>().Object);
        
        var mockBlobService = new Mock<IBlobService>();
        mockBlobService.Setup(s => s.UploadBlob(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IFormFile>()))
            .ReturnsAsync((string blobName, string containerName, IFormFile file) =>
                $"http://127.0.0.1:9090/devstoreaccount1/images/{blobName}");
        mockBlobService.Setup(s => s.DeleteBlob(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(false);
        this.AddSingleton(mockBlobService.Object);
        
        this.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assemblies.Application));
        this.AddValidatorsFromAssembly(Assemblies.Application);
    }
}