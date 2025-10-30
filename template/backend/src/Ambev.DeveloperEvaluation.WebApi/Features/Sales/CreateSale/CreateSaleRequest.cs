namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequest
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public Guid CustomerId { get; set; }

        public Guid SellerId { get; set; }

        public Guid BranchId { get; set; }

        public string BranchName { get; set; } = string.Empty;
    }
}