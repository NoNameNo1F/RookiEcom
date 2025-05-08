using RookiEcom.Modules.Cart.Contracts.Dtos;

namespace RookiEcom.FrontStore.Abstractions;

public interface ICartService
{
    Task<CartDto?> GetCartAsync(CancellationToken cancellationToken = default);
    Task<bool> AddItemToCartAsync(AddCartItemDto item, CancellationToken cancellationToken = default);
    Task<bool> UpdateCartItemAsync(int cartItemId, int quantity, CancellationToken cancellationToken = default);
    Task<bool> RemoveItemFromCartAsync(int cartItemId, CancellationToken cancellationToken = default);
    Task<bool> ClearCartAsync(CancellationToken cancellationToken = default);
}