using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

public class ProductValidatorTests
{
    private readonly ProductValidator _validator;

    public ProductValidatorTests()
    {
        _validator = new ProductValidator();
    }

    [Fact]
    public void Validate_WithValidProduct_ShouldBeValid()
    {
        
        var product = Product.Create("Valid Product", "Valid Description with more than 10 chars", 100.0m, 10);

        var result = _validator.Validate(product);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithLongDescription_ShouldBeInvalid()
    {
        
        var longDescription = new string('a', 501);
        var product = Product.Create("Valid Product", "Normal Description", 100.0m, 10);

        var descriptionProperty = typeof(Product).GetProperty("Description");
        if (descriptionProperty != null && descriptionProperty.CanWrite)
        {
            descriptionProperty.SetValue(product, longDescription);
        }

        var result = _validator.Validate(product);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Description");
    }

    [Fact]
    public void Validate_WithEmptyName_ShouldBeInvalid()
    {
        
        var product = Product.Create("Valid Name", "Valid Description", 100.0m, 10);

        var nameProperty = typeof(Product).GetProperty("Name");
        if (nameProperty != null && nameProperty.CanWrite)
        {
            nameProperty.SetValue(product, "");
        }

        var result = _validator.Validate(product);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validate_WithNegativePrice_ShouldBeInvalid()
    {
        
        var product = Product.Create("Valid Product", "Valid Description", 100.0m, 10);

        var priceProperty = typeof(Product).GetProperty("Price");
        if (priceProperty != null && priceProperty.CanWrite)
        {
            priceProperty.SetValue(product, -10.0m);
        }
        
        var result = _validator.Validate(product);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Price");
    }
}