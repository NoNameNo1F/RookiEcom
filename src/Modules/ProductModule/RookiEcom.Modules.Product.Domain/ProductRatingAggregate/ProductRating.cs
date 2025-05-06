using System;
using RookiEcom.Domain.SeedWork;

namespace RookiEcom.Modules.Product.Domain.ProductRatingAggregate
{
    public class ProductRating : IEntity<int>, IAggregateRoot
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public uint Score { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}
