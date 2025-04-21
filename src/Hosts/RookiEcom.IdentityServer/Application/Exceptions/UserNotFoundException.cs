namespace RookiEcom.IdentityServer.Application.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(Guid userId) : base($"User with Id {userId} was not found.")
    {
    }
}