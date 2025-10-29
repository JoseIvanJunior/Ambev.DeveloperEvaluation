using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;

        public CreateSaleHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var branchName = "Filial Principal";

                var sale = new Sale(
                    request.ProductId,
                    request.Quantity,
                    request.UnitPrice,
                    request.CustomerId,
                    request.SellerId,
                    request.BranchId,
                    branchName
                );

                await _saleRepository.AddAsync(sale);

                return CreateSaleResult.CreateSuccess(sale.Id);
            }
            catch (Exception ex)
            {
                return CreateSaleResult.CreateFailure($"Erro ao criar venda: {ex.Message}");
            }
        }
    }
}