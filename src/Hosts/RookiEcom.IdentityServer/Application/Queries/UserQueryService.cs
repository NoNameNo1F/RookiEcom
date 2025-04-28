using Microsoft.AspNetCore.Identity;
using RookiEcom.Application.Common;
using RookiEcom.IdentityServer.Application.Dtos;

namespace RookiEcom.IdentityServer.Application.Queries;

public class UserQueryService
{
    private readonly IUserRepository _userRepository;

    public UserQueryService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PagedResult<UserDto>> GetCustomersAsync(int pageNumber, int pageSize,
        CancellationToken cancellationToken)
    {
        (IEnumerable<User> users,int count) result = await _userRepository.GetUsersAsync(pageNumber, pageSize, RoleNameConstraints.Customer,cancellationToken);
        var users = new List<UserDto>();
        foreach (var user in result.users)
        {
            users.Add(new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DoB = user.DoB,
                Avatar = user.Avatar,
                Address = user.Address 
            });
        }
        return new PagedResult<UserDto>(users.AsEnumerable(), pageSize, pageNumber, result.count);
    }

    public async Task<UserDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            DoB = user.DoB,
            Avatar = user.Avatar,
            Address = user.Address
        };
    }
}