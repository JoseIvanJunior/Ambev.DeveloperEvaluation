using Xunit;
using NSubstitute;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.UnitTests.Application.TestData;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.UnitTests.Application
{
    public class CreateSaleHandlerTests
    {
        private readonly CreateSaleHandler _handler;
        private readonly ISaleRepository _saleRepository;
        private readonly ISaleFactory _saleFactory;
        private readonly ISaleValidator _saleValidator;
        private readonly ILogger<CreateSaleHandler> _logger;

        public CreateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _saleFactory = Substitute.For<ISaleFactory>();
            _saleValidator = Substitute.For<ISaleValidator>();
            _logger = Substitute.For<ILogger<CreateSaleHandler>>();

            _handler = new CreateSaleHandler(_saleRepository, _saleFactory, _saleValidator, _logger);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ReturnsSuccessResult()
        {
            var command = CreateSaleHandlerTestData.CreateValidCommand();
            var sale = new Sale(command.ProductId, command.Quantity, command.UnitPrice,
                              command.CustomerId, command.SellerId, command.BranchId, "Filial Principal");

            _saleValidator.Validate(command).Returns((true, string.Empty));
            _saleFactory.CreateSale(command).Returns(sale);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Success.Should().BeTrue();
            result.SaleId.Should().Be(sale.Id);
            await _saleRepository.Received(1).AddAsync(Arg.Any<Sale>());
        }

        [Fact]
        public async Task Handle_WithInvalidCommand_ReturnsFailureResult()
        {
            var command = CreateSaleHandlerTestData.CreateValidCommand();
            var errorMessage = "A quantidade deve ser maior que 0";
            _saleValidator.Validate(command).Returns((false, errorMessage));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be(errorMessage);
            await _saleRepository.DidNotReceive().AddAsync(Arg.Any<Sale>());
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrowsException_ReturnsFailureResult()
        {
            var command = CreateSaleHandlerTestData.CreateValidCommand();
            var sale = new Sale(command.ProductId, command.Quantity, command.UnitPrice,
                              command.CustomerId, command.SellerId, command.BranchId, "Filial Principal");

            _saleValidator.Validate(command).Returns((true, string.Empty));
            _saleFactory.CreateSale(command).Returns(sale);

            _saleRepository.When(x => x.AddAsync(Arg.Any<Sale>()))
                .Do(x => throw new Exception("Erro no banco de dados"));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Contain("Erro ao criar venda");
        }

        [Fact]
        public async Task Handle_WithValidCommand_LogsInformationMessages()
        {
            var command = CreateSaleHandlerTestData.CreateValidCommand();
            var sale = new Sale(command.ProductId, command.Quantity, command.UnitPrice,
                              command.CustomerId, command.SellerId, command.BranchId, "Filial Principal");

            _saleValidator.Validate(command).Returns((true, string.Empty));
            _saleFactory.CreateSale(command).Returns(sale);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Success.Should().BeTrue();

            _logger.Received(2).Log(
                Arg.Is<LogLevel>(l => l == LogLevel.Information),
                Arg.Any<EventId>(),
                Arg.Any<object>(),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact]
        public async Task Handle_WithInvalidCommand_LogsWarningMessage()
        {
            var command = CreateSaleHandlerTestData.CreateValidCommand();
            var errorMessage = "A quantidade deve ser maior que 0";
            _saleValidator.Validate(command).Returns((false, errorMessage));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Success.Should().BeFalse();

            _logger.Received(1).Log(
                Arg.Is<LogLevel>(l => l == LogLevel.Warning),
                Arg.Any<EventId>(),
                Arg.Any<object>(),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact]
        public async Task Handle_WhenExceptionOccurs_LogsErrorMessage()
        {
            var command = CreateSaleHandlerTestData.CreateValidCommand();
            var sale = new Sale(command.ProductId, command.Quantity, command.UnitPrice,
                              command.CustomerId, command.SellerId, command.BranchId, "Filial Principal");

            _saleValidator.Validate(command).Returns((true, string.Empty));
            _saleFactory.CreateSale(command).Returns(sale);

            _saleRepository.When(x => x.AddAsync(Arg.Any<Sale>()))
                .Do(x => throw new Exception("Erro no banco de dados"));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Success.Should().BeFalse();

            _logger.Received(1).Log(
                Arg.Is<LogLevel>(l => l == LogLevel.Error),
                Arg.Any<EventId>(),
                Arg.Any<object>(),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }
    }
}