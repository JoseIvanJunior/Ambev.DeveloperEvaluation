using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateProductHandler(_productRepository, _mapper);
    }

    [Fact(DisplayName = "Dados válidos do produto fornecidos ao criar o produto e, em seguida, retorna uma resposta de sucesso")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {

        var command = CreateProductHandlerTestData.GenerateValidCommand();
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            StockQuantity = command.StockQuantity
        };

        var result = new CreateProductResult
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity
        };

        _mapper.Map<Product>(command).Returns(product);
        _mapper.Map<CreateProductResult>(product).Returns(result);
        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(product);

        var createProductResult = await _handler.Handle(command, CancellationToken.None);

        createProductResult.Should().NotBeNull();
        createProductResult.Id.Should().Be(product.Id);
        createProductResult.Name.Should().Be(product.Name);
        createProductResult.Price.Should().Be(product.Price);
        await _productRepository.Received(1).CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Dados de produto inválidos fornecidos ao criar o produto, em seguida, gera uma exceção de validação")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {

        var command = new CreateProductCommand();

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact(DisplayName = "Dado comando válido Ao manipular Então mapeia o comando para a entidade do produto")]
    public async Task Handle_ValidRequest_MapsCommandToProduct()
    {

        var command = CreateProductHandlerTestData.GenerateValidCommand();
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            StockQuantity = command.StockQuantity
        };

        _mapper.Map<Product>(command).Returns(product);
        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(product);

        await _handler.Handle(command, CancellationToken.None);

        _mapper.Received(1).Map<Product>(Arg.Is<CreateProductCommand>(c =>
            c.Name == command.Name &&
            c.Description == command.Description &&
            c.Price == command.Price &&
            c.StockQuantity == command.StockQuantity));
    }

    [Fact(DisplayName = "Dado nome de produto duplicado ao criar produto, então lança exceção")]
    public async Task Handle_DuplicateProductName_ThrowsException()
    {

        var command = CreateProductHandlerTestData.GenerateValidCommand();
        var existingProduct = new Product { Name = command.Name };

        _productRepository.GetByNameAsync(command.Name, Arg.Any<CancellationToken>())
            .Returns(existingProduct);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Produto com nome {command.Name} já existe");
    }
}