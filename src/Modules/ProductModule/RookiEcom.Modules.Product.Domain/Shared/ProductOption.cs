namespace RookiEcom.Modules.Product.Domain.Shared;

public record ProductOption
{
    public string Code { get; set; }
    public List<string> Values { get; set; }
}