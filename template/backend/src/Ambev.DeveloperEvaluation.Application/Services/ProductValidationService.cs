using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Services;

public class ProductValidationService
{
    private readonly IValidator<CreateProductCommand> _createProductValidator;
    private readonly IProductRepository _productRepository;

    public ProductValidationService(
        IValidator<CreateProductCommand> createProductValidator,
        IProductRepository productRepository)
    {
        _createProductValidator = createProductValidator ?? throw new ArgumentNullException(nameof(createProductValidator));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task ValidateProductCreation(CreateProductCommand command, CancellationToken cancellationToken)
    {

        var validationResult = await _createProductValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var existingProduct = await _productRepository.GetByNameAsync(command.Name, cancellationToken);
        if (existingProduct != null)
        {
            throw new InvalidOperationException($"Já existe um produto com o nome: {command.Name}");
        }
    }
}