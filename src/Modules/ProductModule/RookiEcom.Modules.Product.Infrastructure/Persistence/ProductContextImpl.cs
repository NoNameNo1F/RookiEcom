using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RookiEcom.Modules.Product.Application;

namespace RookiEcom.Modules.Product.Infrastructure.Persistence;

public class ProductContextImpl : ProductContext
{
    private readonly ILoggerFactory _loggerFactory;

    public ProductContextImpl(DbContextOptions<ProductContextImpl> options, ILoggerFactory loggerFactory) : base(options)
    {
        _loggerFactory = loggerFactory;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductContextImpl).Assembly);
    }
    
    // public class ProductContextFactory : IDesignTimeDbContextFactory<ProductContextImpl>
    // {
    //     public ProductContextImpl CreateDbContext(string[] args)
    //     {
    //         // var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../Hosts/RookiEcom.WebAPI");
    //
    //         // var configuration = new ConfigurationBuilder()
    //         //     // .SetBasePath(basePath)
    //         //     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    //         //     .Build();
    //
    //         var connectionString =
    //             "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RookiEcom;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;";
    //
    //         var optionsBuilder = new DbContextOptionsBuilder<ProductContextImpl>();
    //         optionsBuilder.UseSqlServer(connectionString,
    //             sql => sql.MigrationsAssembly("RookiEcom.WebAPI"));
    //
    //         return new ProductContextImpl(optionsBuilder.Options, new LoggerFactory());
    //     }
    // }
}


