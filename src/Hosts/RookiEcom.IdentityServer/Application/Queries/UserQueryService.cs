using Microsoft.AspNetCore.Identity;
using RookiEcom.Application.Common;

namespace RookiEcom.IdentityServer.Application.Queries;

public class UserQueryService
{
    private readonly IUserRepository _userRepository;

    public UserQueryService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PagedResult<User>> GetCustomersAsync(int pageNumber, int pageSize,
        CancellationToken cancellationToken)
    {
        (IEnumerable<User> users,int count) result = await _userRepository.GetUsersAsync(pageNumber, pageSize, RoleNameConstraints.Customer,cancellationToken);
        
        return new PagedResult<User>(result.users, pageSize, pageNumber, result.count);
    }

    public async Task<User> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _userRepository.GetUserByIdAsync(userId, cancellationToken);
    }
}