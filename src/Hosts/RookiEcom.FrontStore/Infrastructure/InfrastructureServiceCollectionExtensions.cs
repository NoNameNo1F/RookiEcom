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
            }).AddHttpMessageHandler<IdentityCookieHandler>();
        
        services.AddHttpClient("RookiEcom.IdentityAPI", client =>
        {
            client.BaseAddress = new Uri("https://localhost:8080");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }).AddHttpMessageHandler<IdentityCookieHandler>();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProductRatingService, ProductRatingService>();
        services.AddScoped<ICartService, CartService>();

        return services;
    }
}