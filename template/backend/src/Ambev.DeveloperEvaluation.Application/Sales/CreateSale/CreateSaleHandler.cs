using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public interface ISaleFactory
    {
        Sale CreateSale(CreateSaleCommand request);
    }

    public interface ISaleValidator
    {
        (bool isValid, string errorMessage) Validate(CreateSaleCommand request);
    }

    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ISaleFactory _saleFactory;
        private readonly ISaleValidator _saleValidator;
        private readonly ILogger<CreateSaleHandler> _logger;

        public CreateSaleHandler(
            ISaleRepository saleRepository,
            ISaleFactory saleFactory,
            ISaleValidator saleValidator,
            ILogger<CreateSaleHandler> logger)
        {
            _saleRepository = saleRepository;
            _saleFactory = saleFactory;
            _saleValidator = saleValidator;
            _logger = logger;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Iniciando o processo de criação de vendas para o produto {ProductId}", request.ProductId);

                var validation = _saleValidator.Validate(request);
                if (!validation.isValid)
                {
                    _logger.LogWarning("Falha na validação da venda: {ErrorMessage}", validation.errorMessage);
                    return CreateSaleResult.CreateFailure(validation.errorMessage);
                }

                var sale = _saleFactory.CreateSale(request);

                await _saleRepository.AddAsync(sale);

                _logger.LogInformation("Venda criada com sucesso com ID {SaleId}", sale.Id);

                return CreateSaleResult.CreateSuccess(sale.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar venda para o produto {ProductId}", request.ProductId);
                return CreateSaleResult.CreateFailure($"Erro ao criar venda: {ex.Message}");
            }
        }
    }

    public class SaleFactory : ISaleFactory
    {
        public Sale CreateSale(CreateSaleCommand request)
        {
            var branchName = "Filial Principal";

            var branchId = request.BranchId != Guid.Empty
                ? request.BranchId
                : Guid.NewGuid();

            return new Sale(
                request.ProductId,
                request.Quantity,
                request.UnitPrice,
                request.CustomerId,
                request.SellerId,
                branchId,
                branchName
            );
        }
    }

    public class SaleValidator : ISaleValidator
    {
        private readonly ILogger<SaleValidator> _logger;

        public SaleValidator(ILogger<SaleValidator> logger)
        {
            _logger = logger;
        }

        public (bool isValid, string errorMessage) Validate(CreateSaleCommand request)
        {
            _logger.LogDebug("Validando solicitação de criação de venda");

            if (request.Quantity <= 0)
            {
                return (false, "Quantidade deve ser maior que zero");
            }

            if (request.UnitPrice <= 0)
            {
                return (false, "Preço unitário deve ser maior que zero");
            }

            if (request.ProductId == Guid.Empty)
            {
                return (false, "ProductId é obrigatório");
            }

            if (request.CustomerId == Guid.Empty)
            {
                return (false, "CustomerId é obrigatório");
            }

            if (request.Quantity > 50)
            {
                return (false, "Quantidade máxima por item é 50");
            }

            _logger.LogDebug("Validação de venda concluída com sucesso");
            return (true, string.Empty);
        }
    }
}