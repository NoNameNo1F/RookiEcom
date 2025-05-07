using System.Text.Json;
using RookiEcom.Application.Common;
using RookiEcom.FrontStore.Abstractions;
using RookiEcom.Modules.Product.Contracts.Dtos;

namespace RookiEcom.FrontStore.Infrastructure.Services;

public class ProductRatingService : IProductRatingService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductRatingService> _logger;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public ProductRatingService(IHttpClientFactory httpClientFactory, ILogger<ProductRatingService> logger)
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


    public async Task<PagedResult<ProductRatingDto>?> GetRatingsByProductIdAsync(
        int productId, 
        int pageNumber = 1, 
        int pageSize = 5, 
        CancellationToken cancellationToken = default)
    {
        var request = $"/api/v1/products/{productId}/ratings?pageNumber={pageNumber}&pageSize={pageSize}";
        try
        {
            var response = await _httpClient.GetAsync(request, cancellationToken);
            return await ReadApiResponse<PagedResult<ProductRatingDto>>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error while getting ratings for product ID {ProductId}.", productId);
            return null;
        }
    }

    public async Task<bool> CreateRatingAsync(
        int productId, 
        ProductRatingCreateDto ratingDto, 
        CancellationToken cancellationToken = default)
    {
        var requestUrl = $"/api/v1/products/{productId}/ratings";

        using var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(ratingDto.CustomerId.ToString()), "CustomerId");
        formData.Add(new StringContent(ratingDto.CustomerName ?? string.Empty), "CustomerName");
        formData.Add(new StringContent(ratingDto.Score.ToString()), "Score");
        formData.Add(new StringContent(ratingDto.Content), "Content");
        
        if (ratingDto.Image != null)
        {
            var imageContent = new StreamContent(ratingDto.Image.OpenReadStream());
            imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ratingDto.Image.ContentType);
            formData.Add(imageContent, "Image", ratingDto.Image.FileName);
        }
        
        try
        {
            var response = await _httpClient.PostAsync(requestUrl, formData, cancellationToken);
             var result = await ReadApiResponse<object>(response, cancellationToken, isCreation: true);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error while creating rating for product ID {ProductId}.", productId);
            return false;
        }
    }
}