using System.Text.Json;
using RookiEcom.Modules.Product.Contracts.Dtos;

namespace RookiEcom.FrontStore.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductService> _logger;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public ProductService(IHttpClientFactory httpClientFactory, ILogger<ProductService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("RookiEcom.WebAPI");
        _logger = logger;
    }
    
    private async Task<T?> ReadApiResponse<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            try
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<T>>(_jsonSerializerOptions, cancellationToken);
                return apiResponse != null && apiResponse.StatusCode >= 200 && apiResponse.StatusCode < 300 ? apiResponse.Result : default;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize API response content.");
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogDebug("Response Content: {Content}", content);
                return default;
            }
        }
        else
        {
            _logger.LogError("API request failed with status code {StatusCode}. URI: {RequestUri}", response.StatusCode, response.RequestMessage?.RequestUri);
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("Error Content: {ErrorContent}", errorContent);
            return default;
        }
    }
    
    public async Task<Pagination<ProductDto>?> GetProducts(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var request = $"/api/v1/products?pageNumber={pageNumber}&pageSize={pageSize}";
        
        try
        {
            var response = await _httpClient.GetAsync(request, cancellationToken);
            return await ReadApiResponse<Pagination<ProductDto>>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error while getting products.");
            return null;
        }
    }

    public async Task<Pagination<ProductDto>?> GetProductsByCategoryId(int categoryId, int pageNumber, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var request = $"/api/v1/products/c{categoryId}?pageNumber={pageNumber}&pageSize={pageSize}";
        try
        {
            var response = await _httpClient.GetAsync(request, cancellationToken);
            return await ReadApiResponse<Pagination<ProductDto>>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error while getting products by category ID {CategoryId}.", categoryId);
            return null;
        }
    }

    public async Task<ProductDto?> GetProductById(int productId, CancellationToken cancellationToken = default)
    {
        var request = $"/api/v1/products/{productId}";
        try
        {
            var response = await _httpClient.GetAsync(request, cancellationToken);
            return await ReadApiResponse<ProductDto>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error while getting product by ID {ProductId}.", productId);
            return null;
        }
    }

    public async Task<Pagination<ProductDto>?> GetFeaturedProducts(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var request = $"/api/v1/products/feature";
        try
        {
            var response = await _httpClient.GetAsync(request, cancellationToken);
            return await ReadApiResponse<Pagination<ProductDto>>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error while getting product feature");
            return null;
        }
    }
}