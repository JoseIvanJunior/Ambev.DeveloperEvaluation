using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Services;
using FluentValidation;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Products;

public class CreateProductHandlerTests
{
    private readonly CreateProductHandler _handler;
    private readonly IProductRepository _productRepositoryMock;
    private readonly IValidator<CreateProductCommand> _validatorMock;
    private readonly IMapper _mapperMock;
    private readonly ProductValidationService _validationService;

    public CreateProductHandlerTests()
    {
        _productRepositoryMock = Substitute.For<IProductRepository>();
        _validatorMock = Substitute.For<IValidator<CreateProductCommand>>();
        _mapperMock = Substitute.For<IMapper>();

        _validationService = new ProductValidationService(_validatorMock, _productRepositoryMock);

        _handler = new CreateProductHandler(
            _productRepositoryMock,
            _validationService,
            _mapperMock);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateProduct()
    {
        
        var command = new CreateProductCommand
        {
            Name = "Produto de teste",
            Description = "Teste Descrição",
            Price = 100.0m,
            StockQuantity = 10
        };

        var product = Product.Create(command.Name, command.Description, command.Price, command.StockQuantity);
        var expectedResult = new CreateProductResult { Id = product.Id, Name = product.Name };

        _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());

        _productRepositoryMock.GetByNameAsync(command.Name, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Product?>(null));

        _mapperMock.Map<Product>(command).Returns(product);
        _productRepositoryMock.CreateAsync(product, Arg.Any<CancellationToken>())
            .Returns(product);
        _mapperMock.Map<CreateProductResult>(product).Returns(expectedResult);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(expectedResult.Id);
        result.Name.Should().Be(expectedResult.Name);

        await _validatorMock.Received(1).ValidateAsync(command, Arg.Any<CancellationToken>());
        await _productRepositoryMock.Received(1).GetByNameAsync(command.Name, Arg.Any<CancellationToken>());
        await _productRepositoryMock.Received(1).CreateAsync(product, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithExistingProductName_ShouldThrowException()
    {
        
        var command = new CreateProductCommand
        {
            Name = "Produto Existente",
            Description = "Teste Descrição",
            Price = 100.0m,
            StockQuantity = 10
        };

        var existingProduct = Product.Create(command.Name, "Descrição existente", 50.0m, 5);

        _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());

        _productRepositoryMock.GetByNameAsync(command.Name, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Product?>(existingProduct));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        await _productRepositoryMock.DidNotReceive().CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithInvalidCommand_ShouldThrowValidationException()
    {
        
        var command = new CreateProductCommand
        {
            Name = "",
            Description = "Teste Descrição",
            Price = -10.0m,
            StockQuantity = 10
        };

        var validationFailures = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("Name", "O nome do produto é obrigatório."),
            new FluentValidation.Results.ValidationFailure("Price", "O preço do produto deve ser maior que 0.")
        };

        var validationResult = new FluentValidation.Results.ValidationResult(validationFailures);

        _validatorMock.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(validationResult);

        
        await Assert.ThrowsAsync<ValidationException>(() =>
            _handler.Handle(command, CancellationToken.None));

        
        await _productRepositoryMock.DidNotReceive().GetByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        await _productRepositoryMock.DidNotReceive().CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }
}