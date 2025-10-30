using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.UnitTests.Application.TestData
{
    public class SimpleSaleValidator : ISaleValidator
    {
        private readonly CreateSaleCommandValidator _fluentValidator = new CreateSaleCommandValidator();

        public (bool isValid, string errorMessage) Validate(CreateSaleCommand command)
        {
            var result = _fluentValidator.Validate(command);

            if (!result.IsValid)
            {
                return (false, result.Errors[0].ErrorMessage);
            }

            return (true, string.Empty);
        }
    }
}