using System;
using RookiEcom.Domain.SeedWork;

namespace RookiEcom.Modules.Cart.Domain.CartAggregate
{
    public class CartItem : IEntity<int>
    {
        public int Id { get; set; }
        public int CartId { get; private set; }
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        public string Image { get; private set; }
    
        private CartItem()
        {
        }

        internal CartItem(int cartId, int productId, string productName, int quantity, decimal price, string image)
        {
            CartId = cartId;
            ProductId = productId;
            ProductName = productName;
            Image = image;
            SetPrice(price);
            SetQuantity(quantity);
        }
        
        public decimal CalculateSubTotal()
        {
            return (decimal)Quantity * Price;
        }
        
        internal void SetPrice(decimal price)
        {
            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Price must be positive.");
            }

            Price = price;
        }
        
        internal void SetQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newQuantity), "Quantity must be positive.");
            }

            Quantity = newQuantity;
        }
        
        internal void IncreaseQuantity(int quantityToAdd)
        {
            if (quantityToAdd <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantityToAdd), "Quantity to add must be positive.");
            
            Quantity += quantityToAdd;
        }
    }
}   