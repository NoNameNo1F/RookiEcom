namespace RookiEcom.FrontStore.ViewModels.Dtos;

public class ProductRatingDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Guid CustomerId { get; set; }
    public uint Score { get; set; }
    public string Content { get; set; }
    public string Image { get; set; }
}