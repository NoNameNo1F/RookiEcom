using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RookiEcom.Modules.Product.Domain.ProductRatingAggregate;

namespace RookiEcom.Modules.Product.Infrastructure.Domain;

public class ProductRatingEntityTypeConfiguration : IEntityTypeConfiguration<ProductRating>
{
    public void Configure(EntityTypeBuilder<ProductRating> builder)
    {
        builder.ToTable("ProductRatings", "catalog");

        builder.HasKey(pr => pr.Id);

        builder.Property(pr => pr.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();
        
        builder.Property(pr => pr.ProductId)
            .HasColumnName("ProductId")
            .IsRequired();
        
        builder.Property(pr => pr.CustomerId)
            .HasColumnName("CustomerId")
            .IsRequired();

        builder.Property(pr => pr.Score)
            .HasColumnName("Score")
            .HasDefaultValue(0);
        
        builder.Property(pr => pr.Image)
            .HasColumnName("Image");

        builder.Property(pr => pr.Content)
            .HasColumnName("Content")
            .HasMaxLength(512);

        builder.HasOne<Product.Domain.ProductAggregate.Product>()
            .WithMany()
            .HasForeignKey(pr => pr.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
