using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RookiEcom.IdentityServer.Domain;

namespace RookiEcom.IdentityServer.MappingConfigurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
            .HasColumnName("FirstName")
            .IsRequired();
        
        builder.Property(u => u.LastName)
            .HasColumnName("LastName")
            .IsRequired();

        builder.Property(u => u.DoB)
            .HasColumnName("DoB");
        
        builder.Property(u => u.Avatar)
            .HasColumnName("Avatar")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.OwnsOne(u => u.Address, b =>
        {
            b.Property(a => a.Street).HasColumnName("AddressStreet").IsRequired(false);
            b.Property(a => a.Ward).HasColumnName("AddressWard").IsRequired(false);
            b.Property(a => a.District).HasColumnName("AddressDistrict").IsRequired(false);
            b.Property(a => a.City).HasColumnName("AddressCity").IsRequired(false);
        });
    }
}