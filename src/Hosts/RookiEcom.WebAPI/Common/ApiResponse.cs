using System.Net;

namespace RookiEcom.WebAPI.Common;

public class ApiResponse
{
    public ApiResponse()
    {
        Errors = new List<string>();
    }

    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; } = true;
    public List<string> Errors { get; set; }
    public object Result { get; set; }
}
