using Microsoft.EntityFrameworkCore;

namespace RookiEcom.IdentityServer.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly IdentityContext _dbContext;

    public UserRepository(IdentityContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<(IEnumerable<User>, int)> GetUsersAsync(int pageNumber, int pageSize, string roleName, CancellationToken cancellationToken)
    {
        var query = _dbContext.Users
            .AsNoTracking()
            .Join(_dbContext.UserRoles,
                u => u.Id,
                ur => ur.UserId,
                (u, ur) => new { User = u, UserRoles = ur })
            .Join(_dbContext.Roles,
                ur => ur.UserRoles.RoleId,
                r => r.Id,
                (ur, r) => new { ur.User, RoleName = r.Name })
            .Where(u => u.RoleName.ToUpper() == roleName.ToUpper());
        
        var users = await query
            .Select(u => new User 
            {
                Id = u.User.Id,
                FirstName = u.User.FirstName,
                LastName = u.User.LastName,
                Avatar = u.User.Avatar,
                Email = u.User.Email,
                Address = u.User.Address
            })
            .OrderBy(u => u.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var count = await query.CountAsync(cancellationToken);
        return (users, count);
    }

    public async Task<User> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        return user;
    }
}