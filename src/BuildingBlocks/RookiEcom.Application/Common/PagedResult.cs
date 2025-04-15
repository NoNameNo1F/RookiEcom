namespace RookiEcom.Application.Common;

public class PagedResult<T> : List<T>
{
    public List<T> Items { get; set; }
    public PageData PageData { get; set; }

    public PagedResult(List<T> items, int pageNumber, int pageSize, int total )
    {
        Items = items;
        PageData = new PageData(pageNumber, pageSize, total);
    }
}
