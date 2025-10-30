using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly ProductValidationService _validationService;
    private readonly IMapper _mapper;

    public CreateProductHandler(
        IProductRepository productRepository,
        ProductValidationService validationService,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _validationService = validationService;
        _mapper = mapper;
    }

    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {

        await _validationService.ValidateProductCreation(command, cancellationToken);

        var product = _mapper.Map<Product>(command);
        var createdProduct = await _productRepository.CreateAsync(product, cancellationToken);

        return _mapper.Map<CreateProductResult>(createdProduct);
    }
}