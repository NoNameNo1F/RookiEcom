// using Microsoft.AspNetCore.Http;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Infrastructure;
// using Microsoft.EntityFrameworkCore.Storage;
// using Moq;
// using RookiEcom.Application.Storage;
//
// namespace RookiEcom.Modules.Product.Application.UnitTests.Categories;
//
// public class CategoryCommandTests
// {
//     private readonly Mock<ProductContext> _mockProductContext;
//     private readonly Mock<IBlobService> _mockBlobService;
//     private readonly Mock<DbSet<Domain.CategoryAggregate.Category>> _mockCategoryDbSet;
//     private readonly Mock<DatabaseFacade> _mockDatabaseFacade;
//     private readonly Mock<IDbContextTransaction> _mockTransaction;
//     
//     public ProductCommandTests()
//     {
//         _mockProductContext = new Mock<ProductContext>(new DbContextOptionsBuilder().Options);
//         _mockBlobService = new Mock<IBlobService>();
//         _mockCategoryDbSet = new Mock<DbSet<Domain.CategoryAggregate.Category>>();
//         _mockDatabaseFacade = new Mock<DatabaseFacade>(_mockProductContext.Object);
//         _mockTransaction = new Mock<IDbContextTransaction>();
//
//         _mockProductContext.Setup(c => c.Categories).Returns(_mockCategoryDbSet.Object);
//         _mockProductContext.Setup(c => c.Database).Returns(_mockDatabaseFacade.Object);
//         _mockProductContext.Setup(c => 
//                 c.SaveChangesAsync(It.IsAny<CancellationToken>()))
//             .ReturnsAsync(1);
//         _mockProductContext.Setup(c => 
//                 c.Database.BeginTransactionAsync(It.IsAny<CancellationToken>()))
//             .ReturnsAsync(_mockTransaction.Object);
//         _mockBlobService.Setup(s =>
//                 s.UploadBlob(
//                     It.IsAny<string>(),
//                     It.IsAny<string>(),
//                     It.IsAny<IFormFile>()))
//             .ReturnsAsync((string blobName, string ContainerName, IFormFile file) =>
//                 $"http://127.0.0.1:9090/devstoreaccount1/images/{blobName}");
//         
//         _mockBlobService.Setup(s => s.DeleteBlob(It.IsAny<string>(), It.IsAny<string>()))
//             .ReturnsAsync(true);
//     }
// }