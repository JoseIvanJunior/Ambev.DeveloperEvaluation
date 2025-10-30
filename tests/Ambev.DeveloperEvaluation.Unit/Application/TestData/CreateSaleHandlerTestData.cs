using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.UnitTests.Application.TestData
{
    public static class CreateSaleHandlerTestData
    {
        private static readonly Faker<CreateSaleCommand> _saleCommandFaker;

        static CreateSaleHandlerTestData()
        {
            _saleCommandFaker = new Faker<CreateSaleCommand>()
                .RuleFor(x => x.ProductId, f => f.Random.Guid())
                .RuleFor(x => x.Quantity, f => f.Random.Int(1, 20))
                .RuleFor(x => x.UnitPrice, f => f.Random.Decimal(1, 1000))
                .RuleFor(x => x.CustomerId, f => f.Random.Guid())
                .RuleFor(x => x.SellerId, f => f.Random.Guid())
                .RuleFor(x => x.BranchId, f => f.Random.Guid());
        }

        public static CreateSaleCommand CreateValidCommand() => _saleCommandFaker.Generate();

        public static CreateSaleCommand CreateCommandWithInvalidQuantity(int quantity)
        {
            var command = _saleCommandFaker.Generate();
            command.Quantity = quantity;
            return command;
        }

        public static CreateSaleCommand CreateCommandWithInvalidPrice(decimal price)
        {
            var command = _saleCommandFaker.Generate();
            command.UnitPrice = price;
            return command;
        }

        public static CreateSaleCommand CreateCommandWithEmptyProductId()
        {
            var command = _saleCommandFaker.Generate();
            command.ProductId = Guid.Empty;
            return command;
        }

        public static CreateSaleCommand CreateCommandWithEmptyCustomerId()
        {
            var command = _saleCommandFaker.Generate();
            command.CustomerId = Guid.Empty;
            return command;
        }

        public static CreateSaleCommand CreateCommandWithEmptySellerId()
        {
            var command = _saleCommandFaker.Generate();
            command.SellerId = Guid.Empty;
            return command;
        }

        public static CreateSaleCommand CreateCommandWithEmptyBranchId()
        {
            var command = _saleCommandFaker.Generate();
            command.BranchId = Guid.Empty;
            return command;
        }

        public static IEnumerable<object[]> GetInvalidQuantities()
        {
            yield return new object[] { 0 };
            yield return new object[] { -1 };
            yield return new object[] { -100 };
            yield return new object[] { 21 };
            yield return new object[] { 50 };
        }

        public static IEnumerable<object[]> GetInvalidPrices()
        {
            yield return new object[] { 0m };
            yield return new object[] { -1m };
            yield return new object[] { -100m };
            yield return new object[] { decimal.MinValue };
        }

        public static IEnumerable<object[]> GetEmptyGuids()
        {
            yield return new object[] { Guid.Empty };
        }
    }
}