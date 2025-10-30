using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Include(s => s.Product)
                .Include(s => s.Customer)
                .Include(s => s.Seller)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _context.Sales
                .Include(s => s.Product)
                .Include(s => s.Customer)
                .Include(s => s.Seller)
                .Where(s => s.Status == SaleStatus.Completed)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetByCustomerAsync(Guid customerId)
        {
            return await _context.Sales
                .Include(s => s.Product)
                .Include(s => s.Customer)
                .Include(s => s.Seller)
                .Where(s => s.CustomerId == customerId && s.Status == SaleStatus.Completed)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetBySellerAsync(Guid sellerId)
        {
            return await _context.Sales
                .Include(s => s.Product)
                .Include(s => s.Customer)
                .Include(s => s.Seller)
                .Where(s => s.SellerId == sellerId && s.Status == SaleStatus.Completed)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Sales
                .Include(s => s.Product)
                .Include(s => s.Customer)
                .Include(s => s.Seller)
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate && s.Status == SaleStatus.Completed)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }

        public async Task AddAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Sale sale)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var sale = await GetByIdAsync(id);
            if (sale != null)
            {
                sale.Cancel();
                await UpdateAsync(sale);
            }
        }
    }
}