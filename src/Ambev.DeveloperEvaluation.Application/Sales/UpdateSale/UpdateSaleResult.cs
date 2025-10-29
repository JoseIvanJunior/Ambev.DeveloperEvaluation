namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleResult
    {
        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }

        public static UpdateSaleResult UpdateSuccess() => new() { Success = true };

        public static UpdateSaleResult UpdateFailure(string errorMessage) => new()
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }
}