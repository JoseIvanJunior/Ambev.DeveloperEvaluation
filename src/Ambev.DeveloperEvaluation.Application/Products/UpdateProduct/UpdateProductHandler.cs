using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateProductCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingProduct = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existingProduct == null)
            throw new InvalidOperationException($"Produto com ID {command.Id} não encontrado");

        // Atualiza apenas os campos fornecidos
        if (!string.IsNullOrEmpty(command.Name))
        {
            // Verifica se outro produto já tem esse nome (apenas se nome foi alterado)
            var productWithSameName = await _productRepository.GetByNameAsync(command.Name, cancellationToken);
            if (productWithSameName != null && productWithSameName.Id != command.Id)
                throw new InvalidOperationException($"Produto com nome {command.Name} já existe");

            existingProduct.Name = command.Name;
        }

        if (!string.IsNullOrEmpty(command.Description))
            existingProduct.Description = command.Description;

        if (command.Price.HasValue)
            existingProduct.Price = command.Price.Value;

        if (command.StockQuantity.HasValue)
            existingProduct.StockQuantity = command.StockQuantity.Value;

        existingProduct.UpdatedAt = DateTime.UtcNow;

        var updatedProduct = await _productRepository.UpdateAsync(existingProduct, cancellationToken);

        return new UpdateProductResult
        {
            Id = updatedProduct.Id,
            Name = updatedProduct.Name,
            Description = updatedProduct.Description,
            Price = updatedProduct.Price,
            StockQuantity = updatedProduct.StockQuantity,
            UpdatedAt = updatedProduct.UpdatedAt
        };
    }
}