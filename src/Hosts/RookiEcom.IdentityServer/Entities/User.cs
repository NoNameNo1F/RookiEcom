using Microsoft.AspNetCore.Identity;
using RookiEcom.Domain.SeedWork;
using RookiEcom.Domain.Shared;

namespace RookiEcom.IdentityServer.Domain;

public class User : IdentityUser<Guid>, IAggregateRoot
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DoB { get; set; }
    public string Avatar { get; set; }
    public Address Address { get; set; }
}