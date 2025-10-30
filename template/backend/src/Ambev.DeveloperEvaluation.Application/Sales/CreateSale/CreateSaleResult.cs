namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleResult
    {
        public bool Success { get; set; }
        
        public Guid SaleId { get; set; }

        public string? ErrorMessage { get; set; }

        public static CreateSaleResult CreateSuccess(Guid saleId) => new()
        {
            Success = true,
            SaleId = saleId
        };

        public static CreateSaleResult CreateFailure(string errorMessage) => new()
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }
}