using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RookiEcom.Modules.Product.Domain.CategoryAggregate;
using RookiEcom.Modules.Product.Domain.ProductAggregate;

namespace RookiEcom.Modules.Product.Infrastructure.Domain;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product.Domain.ProductAggregate.Product>
{
    public void Configure(EntityTypeBuilder<Product.Domain.ProductAggregate.Product> builder)
    {
        builder.ToTable("Products", "catalog");

        builder.HasKey(x => x.Id);

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();
        
        builder.Property<string>(p => p.SKU)
            .HasColumnName("SKU")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property<string>(p => p.Name)
            .HasColumnName("Name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property<string>(p => p.Description)
            .HasColumnName("Description")
            .HasMaxLength(512);

        builder.Property<decimal>(p => p.Price)
            .HasColumnName("Price")
            .HasColumnType("decimal(18,2)");

        builder.Property<decimal>(p => p.MarketPrice)
            .HasColumnName("MarketPrice")
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0);

        builder.Property(p => p.Sold)
            .HasColumnName("Sold")
            .HasDefaultValue(0);

        builder.Property(p => p.StockQuantity)
            .HasColumnName("StockQuantity")
            .HasDefaultValue(0);

        builder.Property<ProductStatus>(p => p.Status)
            .HasColumnName("Status");

        builder.Property(p => p.IsFeature)
            .HasColumnName("IsFeature")
            .HasDefaultValue(0);

        builder.Property<DateTime>(p => p.CreatedDateTime)
            .HasColumnName("CreatedDateTime");

        builder.Property<DateTime>(p => p.UpdatedDateTime)
            .HasColumnName("UpdatedDateTime");
        
        builder.PrimitiveCollection(p => p.Images);
        
        builder.OwnsMany(p => p.ProductAttributes, b =>
        {
            b.Property<string>(pa => pa.Code).HasColumnName("ProductAttributesCode");
            b.Property<string>(pa => pa.Value).HasColumnName("ProductAttributesValue");
        });

        builder.OwnsOne(p => p.ProductOption, b =>
        {
            b.Property<string>(v => v.Code).HasColumnName("ProductOptionCode");
            b.Property(v => v.Values).HasColumnName("ProductOptionValue");
        });

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(p => p.Id)
            .HasDatabaseName("IX_Products_Id");
    }
}
