using System.ComponentModel.DataAnnotations;

namespace RookiEcom.Application.Pagination;

public class PagingRequestDto
{
    [Range(0, int.MaxValue)]
    public int PageNumber { get; set; } = 1;
    [Range(1, int.MaxValue)]
    public int PageSize { get; set; } = 25;
}
