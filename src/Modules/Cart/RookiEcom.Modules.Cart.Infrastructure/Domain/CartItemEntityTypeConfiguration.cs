using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RookiEcom.Modules.Cart.Domain.CartAggregate;

namespace RookiEcom.Modules.Cart.Infrastructure.Domain;

public class CartItemEntityTypeConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems", "cart");

        builder.HasKey(ci => ci.Id);
        builder.Property(ci => ci.Id).ValueGeneratedOnAdd();

        builder.Property(ci => ci.CartId)
            .IsRequired();

        builder.Property(ci => ci.ProductId)
            .IsRequired();
        builder.Property(ci => ci.ProductName)
            .IsRequired().HasMaxLength(100);
        builder.Property(ci => ci.Quantity)
            .IsRequired();
        builder.Property(ci => ci.Price)
            .HasColumnName("UnitPrice")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(ci => ci.Image)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.HasIndex(ci => new { ci.CartId, ci.ProductId })
            .HasDatabaseName("IX_CartItems_CartId_ProductId");
    }
}