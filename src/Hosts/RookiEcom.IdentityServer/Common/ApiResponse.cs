using System.Net;

namespace RookiEcom.IdentityServer.Common;
/// <summary>
/// Represents an API response.
/// </summary>
public class ApiResponse
{
    public ApiResponse()
    {
        Errors = new List<string>();
    }

    /// <summary>
    /// The HTTP status code.
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }
    /// <summary>
    /// The State of Response.
    /// </summary>
    public bool IsSuccess { get; set; } = true;
    /// <summary>
    /// Any errors.
    /// </summary>
    public List<string> Errors { get; set; }
    /// <summary>
    /// The result data.
    /// </summary>
    public object Result { get; set; }
}
