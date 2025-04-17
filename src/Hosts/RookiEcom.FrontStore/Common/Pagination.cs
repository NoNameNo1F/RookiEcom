namespace RookiEcom.FrontStore.Common;

public class Pagination<TResult>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 25;
    public int Count { get; set; }
    public IEnumerable<TResult> Items { get; init; } = [];
}