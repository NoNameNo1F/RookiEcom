namespace RookiEcom.FrontStore.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddHttpClient("RookiEcom.WebAPI", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7670");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            
                    .AddHttpMessageHandler<IdentityCookieHandler>();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductRatingService, ProductRatingService>();

        return services;
    }
}