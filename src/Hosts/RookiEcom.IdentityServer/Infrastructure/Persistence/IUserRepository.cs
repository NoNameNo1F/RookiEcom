namespace RookiEcom.IdentityServer.Infrastructure.Persistence;

public interface IUserRepository
{
    Task<(IEnumerable<User>, int)> GetUsersAsync(int pageNumber, int pageSize, string roleName, CancellationToken cancellationToken);
    Task<User> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
}