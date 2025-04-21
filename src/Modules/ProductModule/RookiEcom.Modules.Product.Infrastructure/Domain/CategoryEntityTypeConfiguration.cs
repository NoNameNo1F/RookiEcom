using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RookiEcom.Modules.Product.Domain.CategoryAggregate;

namespace RookiEcom.Modules.Product.Infrastructure.Domain;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories", "catalog");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(c => c.Name)
            .HasColumnName("Name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasColumnName("Description")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.ParentId)
            .HasColumnName("ParentId");

        builder.Property(c => c.IsPrimary)
            .HasColumnName("IsPrimary")
            .HasDefaultValue(false);

        builder.Property(c => c.HasChild)
            .HasColumnName("HasChild");

        builder.Property(c => c.Image)
            .HasColumnName("Image");
        
        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(c => c.ParentId)
            .IsRequired(false);
    }
}
