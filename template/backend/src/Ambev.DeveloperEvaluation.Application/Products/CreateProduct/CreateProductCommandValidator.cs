using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(product => product.Name)
            .NotEmpty()
            .WithMessage("O nome do produto é obrigatório.")
            .Length(2, 100)
            .WithMessage("O nome do produto deve ter entre 2 e 100 caracteres.");

        RuleFor(product => product.Description)
            .NotEmpty()
            .WithMessage("A descrição do produto é obrigatória.")
            .Length(10, 500)
            .WithMessage("A descrição do produto deve ter entre 10 e 500 caracteres.");

        RuleFor(product => product.Price)
            .GreaterThan(0)
            .WithMessage("O preço do produto deve ser maior que 0.");

        RuleFor(product => product.StockQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("A quantidade em estoque não pode ser negativa.");
    }
}