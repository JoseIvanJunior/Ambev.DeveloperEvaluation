using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

public class ProductValidatorTests
{
    private readonly ProductValidator _validator;

    public ProductValidatorTests()
    {
        _validator = new ProductValidator();
    }

    [Fact(DisplayName = "O produto válido deve passar por todas as regras de validação")]
    public void Given_ValidProduct_When_Validated_Then_ShouldNotHaveErrors()
    {

        var product = ProductTestData.GenerateValidProduct();

        var result = _validator.TestValidate(product);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory(DisplayName = "Nome de produto inválido deve falhar na validação")]
    [InlineData("")]
    [InlineData("ab")]
    public void Given_InvalidProductName_When_Validated_Then_ShouldHaveError(string name)
    {

        var product = ProductTestData.GenerateValidProduct();
        product.Name = name;

        var result = _validator.TestValidate(product);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact(DisplayName = "Nome do produto maior que o comprimento máximo deve falhar na validação")]
    public void Given_ProductNameLongerThanMaximum_When_Validated_Then_ShouldHaveError()
    {

        var product = ProductTestData.GenerateValidProduct();
        product.Name = ProductTestData.GenerateLongProductName();

        var result = _validator.TestValidate(product);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact(DisplayName = "Preço negativo não deve ser validado")]
    public void Given_NegativePrice_When_Validated_Then_ShouldHaveError()
    {

        var product = ProductTestData.GenerateValidProduct();
        product.Price = -10.0m;

        var result = _validator.TestValidate(product);

        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact(DisplayName = "Quantidade de estoque negativa deve falhar na validação")]
    public void Given_NegativeStockQuantity_When_Validated_Then_ShouldHaveError()
    {

        var product = ProductTestData.GenerateValidProduct();
        product.StockQuantity = -5;

        var result = _validator.TestValidate(product);

        result.ShouldHaveValidationErrorFor(x => x.StockQuantity);
    }

    [Fact(DisplayName = "Descrição maior que o comprimento máximo deve falhar na validação")]
    public void Given_DescriptionLongerThanMaximum_When_Validated_Then_ShouldHaveError()
    {

        var product = ProductTestData.GenerateValidProduct();
        product.Description = ProductTestData.GenerateLongDescription();

        var result = _validator.TestValidate(product);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }
}