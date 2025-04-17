using Microsoft.AspNetCore.Authentication;
using Microsoft.Net.Http.Headers;

namespace RookiEcom.FrontStore.Extensions;

public class IdentityCookieHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityCookieHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor?.HttpContext;
        if (httpContext != null)
        {
            var accessToken = await httpContext.GetTokenAsync("access_token");
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Add(HeaderNames.Authorization, $"Bearer {accessToken}");
            }
        }
        
        return await base.SendAsync(request, cancellationToken);
    }
}