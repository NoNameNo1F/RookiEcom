using RookiEcom.FrontStore.ViewModels.UserDtos;

namespace RookiEcom.FrontStore.Abstractions;

public interface IUserService
{
    Task<UserDto?> GetUserProfileAsync(Guid userId, CancellationToken cancellationToken = default);
}