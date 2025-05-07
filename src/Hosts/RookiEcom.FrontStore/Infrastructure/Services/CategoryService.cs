using System.Text.Json;
using RookiEcom.Application.Common;
using RookiEcom.Modules.Product.Contracts.Dtos;

namespace RookiEcom.FrontStore.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CategoryService> _logger;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public CategoryService(IHttpClientFactory httpClientFactory, ILogger<CategoryService> logger)
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
    
    public async Task<PagedResult<CategoryDto>?> GetAllCategories(int pageNumber = 1, int pageSize = 100, CancellationToken cancellationToken = default)
    {
        var request = $"/api/v1/categories?pageNumber={pageNumber}&pageSize={pageSize}";
        
        try
        {
            var response = await _httpClient.GetAsync(request, cancellationToken);
            return await ReadApiResponse<PagedResult<CategoryDto>>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error while getting categories.");
            return null;
        }
    }

    public async Task<IEnumerable<CategoryDto>?> GetCategoryTree(int categoryId, CancellationToken cancellationToken = default)
    {
        var request = $"/api/v1/categories/{categoryId}/tree";
        
        try
        {
            var response = await _httpClient.GetAsync(request, cancellationToken);
            return await ReadApiResponse<IEnumerable<CategoryDto>>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error while getting category tree by ID {CategoryId}.", categoryId);
            return null;
        }
    }
    public async Task<CategoryDto?> GetCategoryById(int categoryId, CancellationToken cancellationToken = default)
    {
        var request = $"/api/v1/categories/{categoryId}";
        
        try
        {
            var response = await _httpClient.GetAsync(request, cancellationToken);
            return await ReadApiResponse<CategoryDto>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error while getting category by ID {categoryId}", categoryId);
            return null;
        }
    }
    
}