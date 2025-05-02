using Microsoft.AspNetCore.Http;
using Moq;

namespace RookiEcom.Modules.Product.Application.UnitTests.Utils;

public static class MockFileHelper
{
    public static Mock<IFormFile> CreateMockFormFile(
        string contentType = "image/jpeg",
        string fileName = $"temp.jpg")
    {
        var mockFile = new Mock<IFormFile>();
        var content = "Test image content";
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        ms.Position = 0;

        mockFile.Setup(m => m.OpenReadStream()).Returns(ms);
        mockFile.Setup(m => m.FileName).Returns(fileName);
        mockFile.Setup(m => m.Length).Returns(ms.Length);
        mockFile.Setup(m => m.ContentType).Returns(contentType);
        return mockFile;
    }
}