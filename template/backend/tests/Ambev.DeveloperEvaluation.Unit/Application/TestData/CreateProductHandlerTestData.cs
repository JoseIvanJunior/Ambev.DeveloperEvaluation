using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

public static class CreateProductHandlerTestData
{
    private static readonly Faker<CreateProductCommand> _productFaker = new Faker<CreateProductCommand>()
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Price, f => f.Finance.Amount(1, 1000, 2))
        .RuleFor(p => p.StockQuantity, f => f.Random.Int(0, 500));

    public static CreateProductCommand GenerateValidCommand()
    {
        return _productFaker.Generate();
    }

    public static CreateProductCommand GenerateInvalidCommand()
    {
        return new CreateProductCommand
        {
            Name = "",
            Description = "",
            Price = -10.0m,
            StockQuantity = -5
        };
    }
}