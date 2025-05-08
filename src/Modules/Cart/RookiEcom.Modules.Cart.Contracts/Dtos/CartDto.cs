using System;
using System.Collections.Generic;

namespace RookiEcom.Modules.Cart.Contracts.Dtos
{
    public class CartDto
    {
        public int Id { get; set; }
        public Guid CustomerId { get; set; }
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        public decimal TotalPrice { get; set; }
    }
}