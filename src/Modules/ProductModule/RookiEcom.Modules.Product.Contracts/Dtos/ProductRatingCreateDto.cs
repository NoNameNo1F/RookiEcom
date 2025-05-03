using Microsoft.AspNetCore.Http;

namespace RookiEcom.Modules.Product.Contracts.Dtos;

public class ProductRatingCreateDto
{
    public int ProductId { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public uint Score { get; set; }
    public string Content { get; set; }
    public IFormFile Image { get; set; }
}