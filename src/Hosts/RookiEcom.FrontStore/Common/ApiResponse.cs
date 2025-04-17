namespace RookiEcom.FrontStore.Common;

public class ApiResponse<TResult>
{
    public TResult Result { get; set; }
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public string[] Errors { get; set; }
}