using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RookiEcom.Modules.Cart.Infrastructure.Persistence;

public class CartContext : DbContext
{
    private readonly ILoggerFactory _loggerFactory;
    public DbSet<Cart.Domain.CartAggregate.Cart> Carts { get; set; }
    public DbSet<Cart.Domain.CartAggregate.CartItem> CartItems { get; set; }
    
    public CartContext(DbContextOptions options, ILoggerFactory loggerFactory) : base(options)
    {
        _loggerFactory = loggerFactory;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CartContext).Assembly);
    }
}