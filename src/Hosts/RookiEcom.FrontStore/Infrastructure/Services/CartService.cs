using System.Text.Json;
using RookiEcom.Modules.Cart.Contracts.Dtos;

namespace RookiEcom.FrontStore.Infrastructure.Services;

public class CartService : ICartService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CartService> _logger;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public CartService(IHttpClientFactory httpClientFactory, ILogger<CartService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("RookiEcom.WebAPI");
        _logger = logger;
    }
    
    private async Task<T?> ReadApiResponse<T>(HttpResponseMessage response, CancellationToken cancellationToken, bool isCreation = false)
    {
        if (response.IsSuccessStatusCode)
        {
            if (isCreation && response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return typeof(T) == typeof(bool) ? (T)(object)true : default;
            }
            
            try
            {
                if (response.Content.Headers.ContentLength == 0)
                {
                    return typeof(T) == typeof(bool) ? (T)(object)true : default;
                }

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<T>>(_jsonSerializerOptions, cancellationToken);
                return apiResponse != null && apiResponse.StatusCode >= 200 && apiResponse.StatusCode < 300 ? apiResponse.Result : default;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize API response content. URI: {RequestUri}", response.RequestMessage?.RequestUri);
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
    
    public async Task<CartDto?> GetCartAsync(CancellationToken cancellationToken = default)
    {
        var request = "/api/v1/carts";
        try
        {
            var response = await _httpClient.GetAsync(request, cancellationToken);
            return await ReadApiResponse<CartDto>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error while getting user cart.");
            return null;
        }
    }

    public async Task<bool> AddItemToCartAsync(AddCartItemDto item, CancellationToken cancellationToken = default)
    {
        var request = $"/api/v1/carts";
        try
        {
            var response = await _httpClient.PostAsJsonAsync(request, item, _jsonSerializerOptions,  cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error while adding item to cart. ProductId: {ProductId}", item.ProductId);
            return false;
        }
    }

    public async Task<bool> UpdateCartItemAsync(int cartItemId, int quantity, CancellationToken cancellationToken = default)
    {
        var request = $"/api/v1/carts/items/{cartItemId}";
        try
        {
            var updateDto = new UpdateCartItemQuantityRequestDto { Quantity = quantity };
            var response = await _httpClient.PutAsJsonAsync(request, updateDto, _jsonSerializerOptions, cancellationToken);
            
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error while updating cart item {CartItemId}.", cartItemId);
            return false;
        }
    }

    public async Task<bool> RemoveItemFromCartAsync(int cartItemId, CancellationToken cancellationToken = default)
    {
        var request = $"/api/v1/carts/items/{cartItemId}";
        try
        {
            var response = await _httpClient.DeleteAsync(request, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error while removing cart item {CartItemId}.", cartItemId);
            return false;
        }
    }

    public async Task<bool> ClearCartAsync(CancellationToken cancellationToken = default)
    {
        var requestUrl = "/api/v1/carts";
        try
        {
            var response = await _httpClient.DeleteAsync(requestUrl, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error while clearing cart.");
            return false;
        }
    }
    
    private async Task LogApiError(HttpResponseMessage response, string operation)
    {
        var errorContent = "N/A";
        try { errorContent = await response.Content.ReadAsStringAsync(); } catch { /* ignore */ }
        _logger.LogError("API request failed while {Operation}. Status: {StatusCode}. URI: {RequestUri}. Content: {ErrorContent}",
            operation, response.StatusCode, response.RequestMessage?.RequestUri, errorContent);
    }
}