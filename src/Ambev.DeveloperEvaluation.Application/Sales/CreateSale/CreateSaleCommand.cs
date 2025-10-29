using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        public string? SaleNumber { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal DiscountPercentage { get; set; }

        public Guid CustomerId { get; set; }

        public Guid SellerId { get; set; }

        public Guid BranchId { get; set; }
    }
}