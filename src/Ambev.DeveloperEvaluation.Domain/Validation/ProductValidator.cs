using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do produto é obrigatório")
            .MinimumLength(3).WithMessage("O nome do produto deve ter pelo menos 3 caracteres")
            .MaximumLength(200).WithMessage("O nome do produto não deve exceder 200 caracteres");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("O preço deve ser maior ou igual a 0");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("A quantidade em estoque deve ser maior ou igual a 0");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("A descrição não deve exceder 1000 caracteres");
    }
}