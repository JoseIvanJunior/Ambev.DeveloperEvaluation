using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;

        public SaleService(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<SaleResponse?> GetByIdAsync(Guid id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale == null)
                return null;

            return MapToResponse(sale);
        }

        public async Task<IEnumerable<SaleResponse>> GetAllAsync()
        {
            var sales = await _saleRepository.GetAllAsync();
            return sales.Select(MapToResponse);
        }

        public async Task<IEnumerable<SaleResponse>> GetByCustomerAsync(Guid customerId)
        {
            var sales = await _saleRepository.GetByCustomerAsync(customerId);
            return sales.Select(MapToResponse);
        }

        public async Task<IEnumerable<SaleResponse>> GetBySellerAsync(Guid sellerId)
        {
            var sales = await _saleRepository.GetBySellerAsync(sellerId);
            return sales.Select(MapToResponse);
        }

        public async Task<IEnumerable<SaleResponse>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var sales = await _saleRepository.GetByDateRangeAsync(startDate, endDate);
            return sales.Select(MapToResponse);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale == null)
                return false;

            await _saleRepository.DeleteAsync(id);
            return true;
        }

        private SaleResponse MapToResponse(Sale sale)
        {
            return new SaleResponse
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber,
                ProductId = sale.ProductId,
                ProductName = sale.Product?.Name ?? "Produto não encontrado",
                Quantity = sale.Quantity,
                UnitPrice = sale.UnitPrice,
                DiscountPercentage = sale.DiscountPercentage,
                DiscountAmount = sale.DiscountAmount,
                ItemTotalAmount = sale.ItemTotalAmount,
                TotalAmount = sale.TotalAmount,
                SaleDate = sale.SaleDate,
                CustomerId = sale.CustomerId,
                CustomerName = sale.Customer?.Username ?? "Cliente não encontrado",
                SellerId = sale.SellerId,
                SellerName = sale.Seller?.Username ?? "Vendedor não encontrado",
                BranchId = sale.BranchId,
                BranchName = sale.BranchName ?? "Filial não encontrada",
                Status = sale.Status.ToString()
            };
        }
    }
}