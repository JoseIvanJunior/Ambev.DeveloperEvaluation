namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

public class ProductListResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }
}