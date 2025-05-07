namespace RookiEcom.Application.Common;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public PageData PageData { get; set; }

    public PagedResult()
    {
    }
    
    public PagedResult(IEnumerable<T> items, int pageNumber, int pageSize, int total )
    {
        Items = items;
        PageData = new PageData(pageNumber, pageSize, total);
    }
}
