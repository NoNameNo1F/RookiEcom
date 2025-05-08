using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RookiEcom.Modules.Cart.Infrastructure.Domain;

public class CartEntityTypeConfiguration : IEntityTypeConfiguration<Cart.Domain.CartAggregate.Cart>
{
    public void Configure(EntityTypeBuilder<Cart.Domain.CartAggregate.Cart> builder)
    {
        builder.ToTable("Carts", "cart");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();
        
        builder.Property<Guid>(c => c.CustomerId)
            .HasColumnName("CustomerId")
            .IsRequired();

        builder.Property<decimal>(c => c.TotalPrice)
            .HasColumnName("TotalPrice")
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0m);

        builder.HasMany(c => c.CartItems)
            .WithOne()
            .HasForeignKey(ci => ci.CartId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        var navigation = builder.Metadata.FindNavigation(nameof(Cart.Domain.CartAggregate.Cart.CartItems));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
        
        builder.HasIndex(p => p.CustomerId)
            .HasDatabaseName("IX_Carts_CustomerId");
    }
}