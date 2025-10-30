using Xunit;
using FluentAssertions;
using Ambev.DeveloperEvaluation.UnitTests.Application.TestData;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.UnitTests.Application
{
    public class SaleValidatorTests
    {
        private readonly ISaleValidator _validator;

        public SaleValidatorTests()
        {
            _validator = new SimpleSaleValidator();
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(20, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        [InlineData(21, false)]
        public void Validate_QuantityRules(int quantity, bool expectedIsValid)
        {
            var command = CreateSaleHandlerTestData.CreateCommandWithInvalidQuantity(quantity);

            var result = _validator.Validate(command);

            result.isValid.Should().Be(expectedIsValid);

            if (!expectedIsValid)
            {
                result.errorMessage.Should().NotBeEmpty();
            }
        }

        [Theory]
        [InlineData(1.0, true)]
        [InlineData(1000.0, true)]
        [InlineData(0, false)]
        [InlineData(-1.0, false)]
        public void Validate_UnitPriceRules(decimal unitPrice, bool expectedIsValid)
        {
            var command = CreateSaleHandlerTestData.CreateCommandWithInvalidPrice(unitPrice);

            var result = _validator.Validate(command);

            result.isValid.Should().Be(expectedIsValid);

            if (!expectedIsValid)
            {
                result.errorMessage.Should().NotBeEmpty();
            }
        }

        [Fact]
        public void Validate_WithEmptyProductId_ReturnsFalse()
        {
            var command = CreateSaleHandlerTestData.CreateCommandWithEmptyProductId();

            var result = _validator.Validate(command);

            result.isValid.Should().BeFalse();
            result.errorMessage.Should().Be("O ID do produto é obrigatório");
        }

        [Fact]
        public void Validate_WithEmptyCustomerId_ReturnsFalse()
        {
            var command = CreateSaleHandlerTestData.CreateCommandWithEmptyCustomerId();

            var result = _validator.Validate(command);

            result.isValid.Should().BeFalse();
            result.errorMessage.Should().Be("O ID do cliente é obrigatório");
        }

        [Fact]
        public void Validate_WithEmptySellerId_ReturnsFalse()
        {
            var command = CreateSaleHandlerTestData.CreateCommandWithEmptySellerId();

            var result = _validator.Validate(command);

            result.isValid.Should().BeFalse();
            result.errorMessage.Should().Be("É necessário o ID do vendedor");
        }

        [Fact]
        public void Validate_WithValidCommand_ReturnsTrue()
        {
            var command = CreateSaleHandlerTestData.CreateValidCommand();

            var result = _validator.Validate(command);

            result.isValid.Should().BeTrue();
            result.errorMessage.Should().BeEmpty();
        }

        [Fact]
        public void Validate_WithInvalidQuantity_ReturnsCorrectErrorMessage()
        {
            var command = CreateSaleHandlerTestData.CreateCommandWithInvalidQuantity(0);

            var result = _validator.Validate(command);

            result.isValid.Should().BeFalse();
            result.errorMessage.Should().Be("A quantidade deve ser maior que 0");
        }

        [Fact]
        public void Validate_WithInvalidUnitPrice_ReturnsCorrectErrorMessage()
        {
            var command = CreateSaleHandlerTestData.CreateCommandWithInvalidPrice(0);

            var result = _validator.Validate(command);

            result.isValid.Should().BeFalse();
            result.errorMessage.Should().Be("O preço unitário deve ser maior que 0");
        }

        [Fact]
        public void Validate_WithExcessiveQuantity_ReturnsCorrectErrorMessage()
        {
            var command = CreateSaleHandlerTestData.CreateCommandWithInvalidQuantity(21);

            var result = _validator.Validate(command);

            result.isValid.Should().BeFalse();
            result.errorMessage.Should().Be("Não é possível vender mais de 20 itens idênticos.");
        }

        [Theory]
        [InlineData(1, 1.0, true)]
        [InlineData(0, 1.0, false)]
        [InlineData(1, 0, false)]
        [InlineData(0, 0, false)]
        [InlineData(21, 1.0, false)]
        [InlineData(1, -1.0, false)]
        public void Validate_CombinationOfQuantityAndPrice(int quantity, decimal unitPrice, bool expectedIsValid)
        {
            var command = CreateSaleHandlerTestData.CreateValidCommand();
            command.Quantity = quantity;
            command.UnitPrice = unitPrice;

            var result = _validator.Validate(command);

            result.isValid.Should().Be(expectedIsValid);
        }
    }
}