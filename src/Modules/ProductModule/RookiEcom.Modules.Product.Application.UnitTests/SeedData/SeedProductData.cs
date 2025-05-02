using Microsoft.Extensions.DependencyInjection;
using RookiEcom.Modules.Product.Domain.CategoryAggregate;
using RookiEcom.Modules.Product.Domain.ProductAggregate;
using RookiEcom.Modules.Product.Infrastructure.Persistence;

namespace RookiEcom.Modules.Product.Application.UnitTests.SeedData;

public class SeedProductData
{
    private readonly IServiceProvider _serviceProvider;

    public SeedProductData(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task SeedProductModuleData(CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ProductContextImpl>();
        
        int laptopCategoryId = 4;
        var categories = new List<Category>
        {
            new Category
            {
                Id = 1,
                Name = "Electronic",
                Description = "Electronics Electronics Electronics ",
                IsPrimary = true,
                ParentId = null,
                CreatedDateTime = new DateTime(2025, 04, 29),
                UpdatedDateTime = new DateTime(2025, 04, 29),
                HasChild = true,
                Image = "http://127.0.0.1:9090/devstoreaccount1/images/d1f08197-c962-4b45-b806-35d2c45c7f02"
            },
            new Category
            {
                Id = 2,
                Name = "Computer",
                Description = "ComputerComputerComputerComputerComputerComputer.",
                IsPrimary = true,
                ParentId = 1,
                CreatedDateTime = new DateTime(2025, 04, 29),
                UpdatedDateTime = new DateTime(2025, 04, 29),
                HasChild = true,
                Image = "http://127.0.0.1:9090/devstoreaccount1/images/d1f08197-c962-4b45-b806-35d2c45c7f02"
            },
            new Category
            {
                Id = 3,
                Name = "Mobile",
                Description = "MobileMobileMobileMobileMobileMobile.",
                IsPrimary = true,
                ParentId = 1,
                CreatedDateTime = new DateTime(2025, 04, 29),
                UpdatedDateTime = new DateTime(2025, 04, 29),
                HasChild = true,
                Image = "http://127.0.0.1:9090/devstoreaccount1/images/d1f08197-c962-4b45-b806-35d2c45c7f02"
            },
            new Category
            {
                Id = 4,
                Name = "Laptop",
                Description = "Laptop Laptop Laptop Laptop Laptop Laptop.",
                IsPrimary = true,
                ParentId = 2,
                CreatedDateTime = new DateTime(2025, 04, 29),
                UpdatedDateTime = new DateTime(2025, 04, 29),
                HasChild = true,
                Image = "http://127.0.0.1:9090/devstoreaccount1/images/d1f08197-c962-4b45-b806-35d2c45c7f02"
            },
        };

        await context.Categories.AddRangeAsync(categories, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        var products = new List<Domain.ProductAggregate.Product>
        {
            new Domain.ProductAggregate.Product
            {
                Id = 1,
                SKU = "ACR-NITRO5-001",
                CategoryId = laptopCategoryId,
                Name = "Acer Nitro 5 AN515-58 Gaming Laptop",
                Description = "Experience intense gaming with the Acer Nitro 5. Features 15.6\" FHD 144Hz IPS Display, Intel Core i5-12500H, NVIDIA GeForce RTX 3050 Ti, 16GB DDR4, 512GB NVMe SSD, Killer Wi-Fi 6.",
                MarketPrice = 1199.99m,
                Price = 999.99m,
                Status = ProductStatus.Available,
                Sold = 25,
                StockQuantity = 50,
                IsFeature = true,
                Images = new List<string> { "/images/laptops/acer-nitro5-1.jpg", "/images/laptops/acer-nitro5-2.jpg" },
                ProductAttributes = new List<ProductAttribute>
                {
                    new ProductAttribute { Code = "CPU", Value = "Intel Core i5-12500H" },
                    new ProductAttribute { Code = "RAM", Value = "16GB DDR4" },
                    new ProductAttribute { Code = "Storage", Value = "512GB NVMe SSD" },
                    new ProductAttribute { Code = "GPU", Value = "NVIDIA GeForce RTX 3050 Ti" },
                    new ProductAttribute { Code = "Display", Value = "15.6\" FHD 144Hz" }
                },
                ProductOption = null,
                CreatedDateTime = DateTime.UtcNow.AddDays(-60),
                UpdatedDateTime = DateTime.UtcNow.AddDays(-5)
            },
            new Domain.ProductAggregate.Product
            {
                Id = 2,
                SKU = "ACR-NITRO7-002",
                CategoryId = laptopCategoryId,
                Name = "Acer Nitro 7 AN715-52 Premium Gaming Laptop",
                Description = "Step up your game with the slimmer Nitro 7. 15.6\" Full HD 144Hz Display, Intel Core i7-10750H, NVIDIA GeForce RTX 2060, 16GB DDR4, 1TB NVMe SSD.",
                MarketPrice = 1499.99m,
                Price = 1349.99m,
                Status = ProductStatus.Available,
                Sold = 10,
                StockQuantity = 30,
                IsFeature = false,
                Images = new List<string> { "/images/laptops/acer-nitro7-1.jpg" },
                ProductAttributes = new List<ProductAttribute>
                {
                    new ProductAttribute { Code = "CPU", Value = "Intel Core i7-10750H" },
                    new ProductAttribute { Code = "RAM", Value = "16GB DDR4" },
                    new ProductAttribute { Code = "Storage", Value = "1TB NVMe SSD" },
                    new ProductAttribute { Code = "GPU", Value = "NVIDIA GeForce RTX 2060" }
                },
                ProductOption = null,
                CreatedDateTime = DateTime.UtcNow.AddDays(-90),
                UpdatedDateTime = DateTime.UtcNow.AddDays(-10)
            },

            // --- Dell ---
            new Domain.ProductAggregate.Product
            {
                Id = 3,
                SKU = "DEL-XPS13-003",
                CategoryId = laptopCategoryId,
                Name = "Dell XPS 13 9310 Laptop",
                Description = "Stunning 13.4-inch FHD+ InfinityEdge display. Powered by Intel Core i7-1185G7, 16GB LPDDR4x RAM, 512GB SSD, Intel Iris Xe Graphics. Premium build and performance.",
                MarketPrice = 1649.00m,
                Price = 1549.00m,
                Status = ProductStatus.Available,
                Sold = 18,
                StockQuantity = 45,
                IsFeature = true,
                Images = new List<string> { "/images/laptops/dell-xps13-1.jpg", "/images/laptops/dell-xps13-2.jpg" },
                ProductAttributes = new List<ProductAttribute>
                {
                    new ProductAttribute { Code = "CPU", Value = "Intel Core i7-1185G7" },
                    new ProductAttribute { Code = "RAM", Value = "16GB LPDDR4x" },
                    new ProductAttribute { Code = "Storage", Value = "512GB SSD" },
                    new ProductAttribute { Code = "Display", Value = "13.4\" FHD+" }
                },
                ProductOption = null,
                CreatedDateTime = DateTime.UtcNow.AddDays(-50),
                UpdatedDateTime = DateTime.UtcNow.AddDays(-8)
            },
            new Domain.ProductAggregate.Product
            {
                Id = 4,
                SKU = "DEL-INSP15-004",
                CategoryId = laptopCategoryId,
                Name = "Dell Inspiron 15 3000 Laptop",
                Description = "Affordable and reliable for everyday tasks. 15.6\" HD Anti-Glare Display, Intel Celeron N4020, 8GB DDR4 RAM, 256GB SSD, Integrated Intel UHD Graphics.",
                MarketPrice = 499.99m,
                Price = 399.99m,
                Status = ProductStatus.Available,
                Sold = 45,
                StockQuantity = 120,
                IsFeature = false,
                Images = new List<string> { "/images/laptops/dell-inspiron15-1.jpg" },
                ProductAttributes = new List<ProductAttribute>
                {
                    new ProductAttribute { Code = "CPU", Value = "Intel Celeron N4020" },
                    new ProductAttribute { Code = "RAM", Value = "8GB DDR4" },
                    new ProductAttribute { Code = "Storage", Value = "256GB SSD" }
                },
                ProductOption = null,
                CreatedDateTime = DateTime.UtcNow.AddDays(-40),
                UpdatedDateTime = DateTime.UtcNow.AddDays(-15)
            },
             new Domain.ProductAggregate.Product
            {
                Id = 5,
                SKU = "DEL-AWX17-005",
                CategoryId = laptopCategoryId,
                Name = "Dell Alienware x17 R2 Gaming Laptop",
                Description = "Ultimate gaming powerhouse. 17.3\" FHD 360Hz Display, Intel Core i9-12900H, NVIDIA GeForce RTX 3080 Ti, 32GB DDR5 RAM, 1TB NVMe SSD, CherryMX Keyboard.",
                MarketPrice = 3679.99m,
                Price = 3499.99m,
                Status = ProductStatus.Available,
                Sold = 5,
                StockQuantity = 15,
                IsFeature = true,
                Images = new List<string> { "/images/laptops/alienware-x17-1.jpg" },
                ProductAttributes = new List<ProductAttribute>
                {
                    new ProductAttribute { Code = "CPU", Value = "Intel Core i9-12900H" },
                    new ProductAttribute { Code = "RAM", Value = "32GB DDR5" },
                    new ProductAttribute { Code = "Storage", Value = "1TB NVMe SSD" },
                    new ProductAttribute { Code = "GPU", Value = "NVIDIA GeForce RTX 3080 Ti" }
                },
                ProductOption = null,
                CreatedDateTime = DateTime.UtcNow.AddDays(-30),
                UpdatedDateTime = DateTime.UtcNow.AddDays(-3)
            },

            // --- ThinkPad ---
            new Domain.ProductAggregate.Product
            {
                Id = 6,
                SKU = "LEN-TPX1-006",
                CategoryId = laptopCategoryId,
                Name = "Lenovo ThinkPad X1 Carbon Gen 10",
                Description = "Ultralight business flagship. 14\" WUXGA IPS Anti-Glare, Intel Core i7-1260P, 16GB LPDDR5 RAM, 1TB PCIe SSD, Intel Iris Xe Graphics. Durable and secure.",
                MarketPrice = 2459.00m,
                Price = 2199.00m,
                Status = ProductStatus.Available,
                Sold = 8,
                StockQuantity = 25,
                IsFeature = false,
                Images = new List<string> { "/images/laptops/thinkpad-x1-1.jpg", "/images/laptops/thinkpad-x1-2.jpg" },
                ProductAttributes = new List<ProductAttribute>
                {
                    new ProductAttribute { Code = "CPU", Value = "Intel Core i7-1260P" },
                    new ProductAttribute { Code = "RAM", Value = "16GB LPDDR5" },
                    new ProductAttribute { Code = "Storage", Value = "1TB PCIe SSD" },
                    new ProductAttribute { Code = "Weight", Value = "2.49 lbs" }
                },
                ProductOption = null,
                CreatedDateTime = DateTime.UtcNow.AddDays(-75),
                UpdatedDateTime = DateTime.UtcNow.AddDays(-20)
            },
            new Domain.ProductAggregate.Product
            {
                Id = 7,
                SKU = "LEN-TPT14-007",
                CategoryId = laptopCategoryId,
                Name = "Lenovo ThinkPad T14 Gen 3 (AMD)",
                Description = "Powerful and versatile business workhorse. 14\" WUXGA IPS Display, AMD Ryzen 7 PRO 6850U, 16GB LPDDR5 RAM, 512GB PCIe SSD, AMD Radeon 680M Graphics.",
                MarketPrice = 1999.00m,
                Price = 1799.00m,
                Status = ProductStatus.Available,
                Sold = 12,
                StockQuantity = 40,
                IsFeature = false,
                Images = new List<string> { "/images/laptops/thinkpad-t14-1.jpg" },
                ProductAttributes = new List<ProductAttribute>
                {
                    new ProductAttribute { Code = "CPU", Value = "AMD Ryzen 7 PRO 6850U" },
                    new ProductAttribute { Code = "RAM", Value = "16GB LPDDR5" },
                    new ProductAttribute { Code = "Storage", Value = "512GB PCIe SSD" }
                },
                ProductOption = null,
                CreatedDateTime = DateTime.UtcNow.AddDays(-80),
                UpdatedDateTime = DateTime.UtcNow.AddDays(-25)
            },
             new Domain.ProductAggregate.Product
            {
                Id = 8,
                SKU = "LEN-TPE15-008",
                CategoryId = laptopCategoryId,
                Name = "Lenovo ThinkPad E15 Gen 4",
                Description = "SMB essential laptop. 15.6\" FHD IPS Anti-Glare, Intel Core i5-1235U, 8GB DDR4 RAM, 256GB PCIe SSD, Integrated Intel Iris Xe Graphics, Windows 11 Pro.",
                MarketPrice = 989.00m,
                Price = 849.00m,
                Status = ProductStatus.Available,
                Sold = 22,
                StockQuantity = 60,
                IsFeature = false,
                Images = new List<string> { "/images/laptops/thinkpad-e15-1.jpg" },
                ProductAttributes = new List<ProductAttribute>
                {
                    new ProductAttribute { Code = "CPU", Value = "Intel Core i5-1235U" },
                    new ProductAttribute { Code = "RAM", Value = "8GB DDR4" },
                    new ProductAttribute { Code = "Storage", Value = "256GB PCIe SSD" }
                },
                ProductOption = null,
                CreatedDateTime = DateTime.UtcNow.AddDays(-45),
                UpdatedDateTime = DateTime.UtcNow.AddDays(-7)
            },
             new Domain.ProductAggregate.Product
            {
                Id = 9,
                SKU = "ACR-ASP5-009",
                CategoryId = laptopCategoryId,
                Name = "Acer Aspire 5 A515-56 Slim Laptop",
                Description = "Everyday computing companion. 15.6\" Full HD IPS Display, Intel Core i5-1135G7, 8GB DDR4, 512GB NVMe SSD, Intel Iris Xe Graphics, Wi-Fi 6, Backlit KB.",
                MarketPrice = 649.99m,
                Price = 579.99m,
                Status = ProductStatus.Available,
                Sold = 35,
                StockQuantity = 75,
                IsFeature = false,
                Images = new List<string> { "/images/laptops/acer-aspire5-1.jpg" },
                ProductAttributes = new List<ProductAttribute>
                {
                    new ProductAttribute { Code = "CPU", Value = "Intel Core i5-1135G7" },
                    new ProductAttribute { Code = "RAM", Value = "8GB DDR4" },
                    new ProductAttribute { Code = "Storage", Value = "512GB NVMe SSD" }
                },
                ProductOption = null,
                CreatedDateTime = DateTime.UtcNow.AddDays(-55),
                UpdatedDateTime = DateTime.UtcNow.AddDays(-9)
            },
             new Domain.ProductAggregate.Product
            {
                Id = 10,
                SKU = "ACR-NITRO5-OOS-010",
                CategoryId = laptopCategoryId,
                Name = "Acer Nitro 5 AN515-57 (Older Model)",
                Description = "Popular budget gaming laptop (Previous Gen). 15.6\" FHD 144Hz, Intel Core i5-11400H, NVIDIA GeForce GTX 1650, 8GB DDR4, 256GB NVMe SSD.",
                MarketPrice = 839.99m,
                Price = 749.99m,
                Status = ProductStatus.Removed,
                Sold = 150,
                StockQuantity = 0,
                IsFeature = false,
                Images = new List<string> { "/images/laptops/acer-nitro-old-1.jpg" },
                ProductAttributes = new List<ProductAttribute>
                {
                    new ProductAttribute { Code = "CPU", Value = "Intel Core i5-11400H" },
                    new ProductAttribute { Code = "RAM", Value = "8GB DDR4" },
                    new ProductAttribute { Code = "Storage", Value = "256GB NVMe SSD" },
                    new ProductAttribute { Code = "GPU", Value = "NVIDIA GeForce GTX 1650" }
                },
                ProductOption = null,
                CreatedDateTime = DateTime.UtcNow.AddDays(-300),
                UpdatedDateTime = DateTime.UtcNow.AddDays(-30)
            }
        };

        await context.AddRangeAsync(products, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}