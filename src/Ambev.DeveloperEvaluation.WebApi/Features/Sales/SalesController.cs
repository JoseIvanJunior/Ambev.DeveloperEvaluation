using Microsoft.AspNetCore.Mvc;
using MediatR;
using Serilog;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Common.Responses;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISaleService _saleService;
        private readonly ILogger<SalesController> _logger;

        public SalesController(IMediator mediator, ISaleService saleService, ILogger<SalesController> logger)
        {
            _mediator = mediator;
            _saleService = saleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetSaleResponse>>>> GetAll()
        {
            var sales = await _saleService.GetAllAsync();
            var response = sales.Select(SaleMapper.MapToGetSaleResponse);
            _logger.LogInformation("Recuperado {Count} vendas", response.Count());
            return Ok(ApiResponse.Success(response));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<GetSaleResponse>>> GetById(Guid id)
        {
            var sale = await _saleService.GetByIdAsync(id);
            if (sale == null)
            {
                _logger.LogWarning("Venda com ID {Id} não encontrado", id);
                return NotFound(ApiResponse.Fail<GetSaleResponse>("Venda não encontrada."));
            }

            _logger.LogInformation("Venda recuperada com ID {Id}", id);
            return Ok(ApiResponse.Success(SaleMapper.MapToGetSaleResponse(sale)));
        }

        [HttpGet("customer/{customerId:guid}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetSaleResponse>>>> GetByCustomer(Guid customerId)
        {
            var sales = await _saleService.GetByCustomerAsync(customerId);
            var response = sales.Select(SaleMapper.MapToGetSaleResponse);
            _logger.LogInformation("Recuperado {Count} vendas para cliente {CustomerId}", response.Count(), customerId);
            return Ok(ApiResponse.Success(response));
        }

        [HttpGet("seller/{sellerId:guid}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetSaleResponse>>>> GetBySeller(Guid sellerId)
        {
            var sales = await _saleService.GetBySellerAsync(sellerId);
            var response = sales.Select(SaleMapper.MapToGetSaleResponse);
            _logger.LogInformation("Recuperado {Count} vendas para vendedor {SellerId}", response.Count(), sellerId);
            return Ok(ApiResponse.Success(response));
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetSaleResponse>>>> GetByDateRange(
            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var sales = await _saleService.GetByDateRangeAsync(startDate, endDate);
            var response = sales.Select(SaleMapper.MapToGetSaleResponse);
            _logger.LogInformation("Recuperado {Count} vendas entre {StartDate} e {EndDate}",
                response.Count(), startDate, endDate);
            return Ok(ApiResponse.Success(response));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CreateSaleResponse>>> Create([FromBody] CreateSaleRequest request)
        {
            var command = new CreateSaleCommand
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice,
                CustomerId = request.CustomerId,
                SellerId = request.SellerId,
                BranchId = request.BranchId != Guid.Empty ? request.BranchId : Guid.Empty
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                _logger.LogWarning("Falha ao criar a venda: {Message}", result.ErrorMessage);
                return BadRequest(ApiResponse.Fail<CreateSaleResponse>(result.ErrorMessage ?? "Erro ao criar venda."));
            }

            _logger.LogInformation("Venda criada com ID {SaleId}", result.SaleId);
            return CreatedAtAction(nameof(GetById),
                new { id = result.SaleId },
                ApiResponse.Success(new CreateSaleResponse
                {
                    Success = true,
                    SaleId = result.SaleId,
                    Message = "Venda criada com sucesso."
                }));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(Guid id, [FromBody] UpdateSaleRequest request)
        {
            var command = new UpdateSaleCommand
            {
                SaleId = id,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                _logger.LogWarning("Falha ao atualizar a venda {Id}: {Message}", id, result.ErrorMessage);
                return BadRequest(ApiResponse.Fail<object>(result.ErrorMessage ?? "Erro ao atualizar venda."));
            }

            _logger.LogInformation("Venda atualizada {Id}", id);
            return Ok(ApiResponse.Success<object>(null, "Venda atualizada com sucesso."));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<DeleteSaleResponse>>> Delete(Guid id)
        {
            var result = await _saleService.DeleteAsync(id);

            if (!result)
            {
                _logger.LogWarning("Tentativa de excluir venda {Id}, mas não foi encontrado.", id);
                return NotFound(ApiResponse.Fail<DeleteSaleResponse>("Venda não encontrada."));
            }

            _logger.LogInformation("Venda excluída {Id}", id);
            return Ok(ApiResponse.Success(new DeleteSaleResponse
            {
                Success = true,
                Message = "Venda excluída com sucesso."
            }));
        }
    }

    internal static class SaleMapper
    {
        public static GetSaleResponse MapToGetSaleResponse(SaleResponse s) => new GetSaleResponse
        {
            Id = s.Id,
            SaleNumber = s.SaleNumber,
            ProductId = s.ProductId,
            ProductName = s.ProductName,
            Quantity = s.Quantity,
            UnitPrice = s.UnitPrice,
            DiscountPercentage = s.DiscountPercentage,
            DiscountAmount = s.DiscountAmount,
            ItemTotalAmount = s.ItemTotalAmount,
            TotalAmount = s.TotalAmount,
            SaleDate = s.SaleDate,
            CustomerId = s.CustomerId,
            CustomerName = s.CustomerName,
            SellerId = s.SellerId,
            SellerName = s.SellerName,
            BranchId = s.BranchId,
            BranchName = s.BranchName,
            Status = s.Status
        };
    }
}
