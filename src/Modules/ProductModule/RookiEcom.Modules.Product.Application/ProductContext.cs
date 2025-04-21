using Microsoft.EntityFrameworkCore;
using RookiEcom.Modules.Product.Domain.CategoryAggregate;
using RookiEcom.Modules.Product.Domain.ProductRatingAggregate;

namespace RookiEcom.Modules.Product.Application;

public abstract class ProductContext : DbContext
{
    public DbSet<Domain.ProductAggregate.Product> Products { get; set; }
    public DbSet<ProductRating> ProductRatings { get; set; }
    public DbSet<Category> Categories { get; set; }

    public ProductContext(DbContextOptions options) : base(options)
    {
    }
}