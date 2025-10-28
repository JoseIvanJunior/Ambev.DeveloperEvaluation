using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public UpdateProductHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var existingProduct = await _productRepository.GetByIdAsync(command.Id, cancellationToken);

        if (existingProduct == null)
            throw new KeyNotFoundException($"Produto com id {command.Id} não encontrado");

        var name = command.Name ?? existingProduct.Name;
        var description = command.Description ?? existingProduct.Description;
        var price = command.Price ?? existingProduct.Price;
        var stockQuantity = command.StockQuantity ?? existingProduct.StockQuantity;

        existingProduct.Update(name, description, price, stockQuantity);

        var updatedProduct = await _productRepository.UpdateAsync(existingProduct, cancellationToken);

        return _mapper.Map<UpdateProductResult>(updatedProduct);
    }
}