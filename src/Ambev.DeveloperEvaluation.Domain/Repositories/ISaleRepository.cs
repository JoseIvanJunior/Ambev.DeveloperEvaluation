using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository
    {
        Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<Sale>> GetAllAsync();

        Task<IEnumerable<Sale>> GetByCustomerAsync(Guid customerId);

        Task<IEnumerable<Sale>> GetBySellerAsync(Guid sellerId);

        Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        Task AddAsync(Sale sale);

        Task UpdateAsync(Sale sale);

        Task DeleteAsync(Guid id);
    }
}