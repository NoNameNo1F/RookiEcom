namespace RookiEcom.Modules.Cart.Application.Abstraction
{
    public interface ICartRepository
    {
        Task<Domain.CartAggregate.Cart> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
        Task AddAsync(Domain.CartAggregate.Cart cart, CancellationToken cancellationToken = default);
        Task UpdateAsync(Domain.CartAggregate.Cart cart, CancellationToken cancellationToken = default);
    }
}