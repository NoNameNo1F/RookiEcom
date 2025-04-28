using System.Text.Json;
using RookiEcom.FrontStore.ViewModels.UserDtos;

namespace RookiEcom.FrontStore.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserService> _logger;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);
    
    public UserService(IHttpClientFactory httpClientFactory, ILogger<UserService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("RookiEcom.IdentityAPI");
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
    
    public async Task<UserDto?> GetUserProfileAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var request = $"/api/v1/users/{userId}";
        try
        {
            var response = await _httpClient.GetAsync(request, cancellationToken);
            return await ReadApiResponse<UserDto>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error while getting profile by ID {UserId}.", userId);
            return null;
        }
    }
}