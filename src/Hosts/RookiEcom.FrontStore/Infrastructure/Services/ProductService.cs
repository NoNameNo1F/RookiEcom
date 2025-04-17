using System.Text.Json;
using RookiEcom.FrontStore.Abstractions;
using RookiEcom.FrontStore.ViewModels.ProductDtos;

namespace RookiEcom.FrontStore.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Pagination<ProductDto>> GetProducts(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient("RookiEcom.WebAPI");
        var request = $"/api/v1/products?pageNumber={pageNumber}&pageSize={pageSize}";
        
        var response = await client.GetAsync(request);
        response.EnsureSuccessStatusCode();
        
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<Pagination<ProductDto>>>(cancellationToken: cancellationToken);
        if (apiResponse == null || !apiResponse.IsSuccess)
        {
            throw new Exception("Failed to fetch products from the API.");
        }
        
        return apiResponse.Result;
    }
}