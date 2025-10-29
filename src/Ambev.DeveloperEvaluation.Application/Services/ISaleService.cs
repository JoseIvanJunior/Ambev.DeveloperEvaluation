using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Application.Services
{
    public interface ISaleService
    {
        Task<SaleResponse?> GetByIdAsync(Guid id);

        Task<IEnumerable<SaleResponse>> GetAllAsync();

        Task<IEnumerable<SaleResponse>> GetByCustomerAsync(Guid customerId);

        Task<IEnumerable<SaleResponse>> GetBySellerAsync(Guid sellerId);

        Task<IEnumerable<SaleResponse>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        Task<bool> DeleteAsync(Guid id);
    }
}