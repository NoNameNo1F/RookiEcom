using RookiEcom.Application.Common;
using RookiEcom.Modules.Product.Contracts.Dtos;

namespace RookiEcom.FrontStore.Abstractions;

public interface IProductRatingService
{
    Task<PagedResult<ProductRatingDto>?> GetRatingsByProductIdAsync(
        int productId, 
        int pageNumber = 1, 
        int pageSize = 5,
        CancellationToken cancellationToken = default);

    Task<bool> CreateRatingAsync(
        int productId, 
        ProductRatingCreateDto ratingDto, 
        CancellationToken cancellationToken = default);
}