using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
    {
        public UpdateSaleCommandValidator()
        {
            RuleFor(x => x.SaleId)
                .NotEmpty().WithMessage("O ID da venda é obrigatório");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("A quantidade deve ser maior que 0")
                .LessThanOrEqualTo(20).WithMessage("Não é possível vender mais de 20 itens idênticos.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("O preço unitário deve ser maior que 0");
        }
    }
}