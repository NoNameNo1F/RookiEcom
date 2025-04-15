namespace RookiEcom.Domain.Shared;

public record Address
{
    public string Street { get; set; }
    public string District { get; set; }
    public string Ward { get; set; }
    public string City { get; set; }
}