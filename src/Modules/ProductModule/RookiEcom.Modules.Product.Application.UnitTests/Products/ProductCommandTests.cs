using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using RookiEcom.Application.Storage;
using RookiEcom.Modules.Product.Application.Commands.Product.Create;
using RookiEcom.Modules.Product.Application.UnitTests.Utils;
using RookiEcom.Modules.Product.Domain.ProductAggregate;

namespace RookiEcom.Modules.Product.Application.UnitTests.Products;

public class ProductCommandTests
{
    private readonly Mock<ProductContext> _mockProductContext;
    private readonly Mock<IBlobService> _mockBlobService;
    private readonly Mock<DbSet<Domain.ProductAggregate.Product>> _mockProductDbSet;
    private readonly Mock<DatabaseFacade> _mockDatabaseFacade;
    private readonly Mock<IDbContextTransaction> _mockTransaction;

    public ProductCommandTests()
    {
        _mockProductContext = new Mock<ProductContext>(new DbContextOptionsBuilder().Options);
        _mockBlobService = new Mock<IBlobService>();
        _mockProductDbSet = new Mock<DbSet<Domain.ProductAggregate.Product>>();
        _mockDatabaseFacade = new Mock<DatabaseFacade>(_mockProductContext.Object);
        _mockTransaction = new Mock<IDbContextTransaction>();

        _mockProductContext.Setup(c => c.Products).Returns(_mockProductDbSet.Object);
        _mockProductContext.Setup(c => c.Database).Returns(_mockDatabaseFacade.Object);
        _mockProductContext.Setup(c => 
                c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        _mockProductContext.Setup(c => 
                c.Database.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockTransaction.Object);
        _mockBlobService.Setup(s =>
                s.UploadBlob(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<IFormFile>()))
            .ReturnsAsync((string blobName, string ContainerName, IFormFile file) =>
                $"http://127.0.0.1:9090/devstoreaccount1/images/{blobName}");
        
        _mockBlobService.Setup(s => s.DeleteBlob(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);
    }
    
    [Fact]
    public async Task CreateProduct_ValidCreateProductCommand_ReturnsCreatedProductAndImages()
    {
        // Arrange
        var mockFile1 = MockFileHelper.CreateMockFormFile(fileName: "product1.jpg");
        var mockFile2 = MockFileHelper.CreateMockFormFile(fileName: "product2.jpg");
        var command = new CreateProductCommand(
            sku: "TEST-SKU-01",
            categoryId: 1,
            name: "Test Product",
            description: "Test Desc",
            marketPrice: 100m,
            price: 80m,
            stockQuantity: 10,
            isFeature: false,
            images: new[] { mockFile1.Object, mockFile2.Object },
            productAttributes: new List<ProductAttribute>
                { new ProductAttribute { Code = "Color", Value = "Red" } },
            productOption: new ProductOption
                { Code = "Size", Values = new List<string> { "M", "L" } }
        );

        var handler = new CreateProductCommandHandler(_mockProductContext.Object, _mockBlobService.Object);
        
        // Act
        await handler.Handle(command, CancellationToken.None);
        
        // Assert
        _mockProductDbSet.Verify(db => 
            db.Add(It.IsAny<Domain.ProductAggregate.Product>()), Times.Once);
        _mockProductContext.Verify(db => 
            db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockTransaction.Verify(t => 
            t.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockTransaction.Verify(t => 
            t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Never);
        
        _mockBlobService.Verify(bs => 
            bs.UploadBlob(
                It.IsAny<string>(), "images", It.IsAny<IFormFile>()), 
                Times.Exactly(command.Images.Length));
        _mockBlobService.Verify(bs => 
            bs.DeleteBlob(It.IsAny<string>(), "images"),
            Times.Never);
    }
}
