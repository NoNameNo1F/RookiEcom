using System.ComponentModel.DataAnnotations;

namespace RookiEcom.Modules.Cart.Contracts.Dtos
{
    public class UpdateCartItemQuantityRequestDto
    {
        [Required]
        [Range(0, 99)]
        public int Quantity { get; set; }
    }
}