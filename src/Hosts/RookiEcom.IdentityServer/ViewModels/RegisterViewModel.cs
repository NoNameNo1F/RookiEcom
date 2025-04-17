using System.ComponentModel.DataAnnotations;

namespace RookiEcom.IdentityServer.ViewModels;

public class RegisterViewModel
{
    [Required, MaxLength(20)]
    public string FirstName { get; init; }
    [Required, MaxLength(20)]
    public string LastName { get; init; }
    [Required, MaxLength(100), EmailAddress]
    public string Email { get; init; }
    
    [Required, MaxLength(100)]
    public string UserName { get; set; }
    
    [Required]
    public DateTime DoB { get; set; }
    
    [Required, MinLength(8)]
    public string Password { get; set; }
    [Required, MinLength(8)]
    public string RePassword { get; set; }
    public string ReturnUrl { get; set; }
}