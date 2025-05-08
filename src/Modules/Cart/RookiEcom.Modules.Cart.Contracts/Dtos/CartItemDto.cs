namespace RookiEcom.Modules.Cart.Contracts.Dtos
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public decimal SubTotal { get; set; }
    }
}