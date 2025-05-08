using System;
using System.Collections.Generic;
using System.Linq;
using RookiEcom.Domain.SeedWork;

namespace RookiEcom.Modules.Cart.Domain.CartAggregate
{
    public class Cart : IEntity<int>, IAggregateRoot
    {
        public int Id { get; set; }
        public Guid CustomerId { get; private set; }

        private readonly List<CartItem> _cartItems = new List<CartItem>();
        public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();
        public decimal TotalPrice { get; private set; }

        private Cart()
        {
        }

        public Cart(Guid customerId)
        {
            CustomerId = customerId;
            CalculateTotalPrice();
        }

        public void AddItem(int productId, string productName, int quantity, decimal price, string image)
        {
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            
            var existingItem = _cartItems.FirstOrDefault(item => item.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
            }
            else
            {
                var newItem = new CartItem(this.Id, productId, productName, quantity, price, image);
                _cartItems.Add(newItem);
            }
            CalculateTotalPrice();
        }

        public void UpdateItemQuantity(int cartItemId, int newQuantity)
        {
            var itemToUpdate = _cartItems.FirstOrDefault(item => item.Id == cartItemId);
            if (itemToUpdate == null)
            {
                return;
            }

            if (newQuantity <= 0)
            {
                RemoveItem(cartItemId);
            }
            else
            {
                itemToUpdate.SetQuantity(newQuantity);
                CalculateTotalPrice();
            }
        }
        
        public void RemoveItem(int cartItemId)
        {
            var itemToRemove = _cartItems.FirstOrDefault(item => item.Id == cartItemId);
            if (itemToRemove != null)
            {
                _cartItems.Remove(itemToRemove);
                CalculateTotalPrice();
            }
        }
        
        public void ClearItems()
        {
            _cartItems.Clear();
            CalculateTotalPrice();
        }
        
        public void CalculateTotalPrice()
        {
            TotalPrice = _cartItems.Sum(item => item.CalculateSubTotal());
        }
    }
}