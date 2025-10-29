using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;

        public UpdateSaleHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(request.SaleId, cancellationToken);
            if (sale == null)
                return UpdateSaleResult.UpdateFailure("Venda não encontrada");

            try
            {
                var oldTotalAmount = sale.TotalAmount;

                sale.SetQuantity(request.Quantity);
                sale.SetUnitPrice(request.UnitPrice);

                if (sale.TotalAmount != oldTotalAmount)
                {
                    sale.AddDomainEvent(new SaleModifiedEvent(sale.Id, sale.SaleNumber, sale.TotalAmount));
                }

                await _saleRepository.UpdateAsync(sale);

                return UpdateSaleResult.UpdateSuccess();
            }
            catch (DomainException ex)
            {
                return UpdateSaleResult.UpdateFailure(ex.Message);
            }
            catch (Exception ex)
            {
                return UpdateSaleResult.UpdateFailure($"Erro interno: {ex.Message}");
            }
        }
    }
}