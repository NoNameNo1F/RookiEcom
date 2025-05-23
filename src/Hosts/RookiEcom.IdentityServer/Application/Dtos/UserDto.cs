﻿using RookiEcom.Domain.Shared;

namespace RookiEcom.IdentityServer.Application.Dtos;

public class UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DoB { get; set; }
    public string Avatar { get; set; }
    public Address Address { get; set; }
}