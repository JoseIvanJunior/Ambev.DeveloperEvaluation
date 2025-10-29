namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleResponse
    {
        public bool Success { get; set; }

        public Guid SaleId { get; set; }

        public string? Message { get; set; }
    }
}