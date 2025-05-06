using Microsoft.AspNetCore.Http;
using RookiEcom.Application.Contracts;

namespace RookiEcom.Modules.Product.Application.Commands.ProductRating.Create;

public sealed class CreateProductRatingCommand : CommandBase
{
    public int ProductId { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public uint Score { get; set; }
    public string Content { get; set; }
    public IFormFile? Image { get; set; }
}