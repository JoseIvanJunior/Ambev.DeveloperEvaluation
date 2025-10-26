using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        When(x => !string.IsNullOrEmpty(x.Name), () => {
            RuleFor(x => x.Name)
                .Length(2, 100).WithMessage("O nome do produto deve ter entre 2 e 100 caracteres.");
        });

        When(x => !string.IsNullOrEmpty(x.Description), () => {
            RuleFor(x => x.Description)
                .Length(10, 500).WithMessage("A descrição do produto deve ter entre 10 e 500 caracteres.");
        });

        When(x => x.Price.HasValue, () => {
            RuleFor(x => x.Price!.Value)
                .GreaterThan(0).WithMessage("O preço do produto deve ser maior que 0.");
        });

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("A quantidade em estoque não pode ser negativa.")
            .When(x => x.StockQuantity.HasValue);
    }
}