using System.ComponentModel.DataAnnotations;

namespace RookiEcom.Application.Pagination;

/// <summary>
/// Represents a pagination request.
/// </summary>
public class PagingRequestDto
{
    /// <summary>
    /// The page number (0-based).
    /// </summary>
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;
    /// <summary>
    /// The page size (minimum 1).
    /// </summary>
    [Range(1, int.MaxValue)]
    public int PageSize { get; set; } = 25;
}
