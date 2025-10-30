using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken);

    Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken);

    Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken);

    Task<Product> CreateAsync(Product product, CancellationToken cancellationToken);

    Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}