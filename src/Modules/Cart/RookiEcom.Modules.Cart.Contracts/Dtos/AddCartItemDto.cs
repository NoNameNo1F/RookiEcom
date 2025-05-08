namespace RookiEcom.Modules.Cart.Contracts.Dtos
{
    public class AddCartItemDto
    {
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
    }
}