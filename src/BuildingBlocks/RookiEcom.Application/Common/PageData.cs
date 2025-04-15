namespace RookiEcom.Application.Common;

public class PageData
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }

    public bool HasPrevious => PageNumber > 0;
    public bool HasNext => PageNumber < TotalPages - 1;

    public PageData(int pageNumber, int pageSize, int totalCount)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        TotalCount = totalCount;
    }
}
