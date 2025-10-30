namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class SaleResponse
    {
        public Guid Id { get; set; }

        public string? SaleNumber { get; set; }

        public Guid ProductId { get; set; }

        public string? ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal DiscountPercentage { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal ItemTotalAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime SaleDate { get; set; }

        public Guid CustomerId { get; set; }

        public string? CustomerName { get; set; }

        public Guid SellerId { get; set; }

        public string? SellerName { get; set; }

        public Guid BranchId { get; set; }

        public string? BranchName { get; set; }

        public string? Status { get; set; }
    }
}