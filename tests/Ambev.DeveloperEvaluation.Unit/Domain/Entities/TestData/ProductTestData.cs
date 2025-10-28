using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class ProductTestData
{
    private static readonly Faker<Product> _productFaker = new Faker<Product>()
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Price, f => f.Finance.Amount(1, 1000, 2))
        .RuleFor(p => p.StockQuantity, f => f.Random.Int(0, 500));

    public static Product GenerateValidProduct()
    {
        return _productFaker.Generate();
    }

    public static string GenerateValidProductName()
    {
        return new Faker().Commerce.ProductName();
    }

    public static string GenerateLongProductName()
    {
        return new Faker().Random.String2(201);
    }

    public static string GenerateLongDescription()
    {
        return new Faker().Random.String2(1001);
    }

    public static decimal GenerateNegativePrice()
    {
        return new Faker().Finance.Amount(-100, -1, 2);
    }

    public static int GenerateNegativeStock()
    {
        return new Faker().Random.Int(-100, -1);
    }
}