using Microsoft.AspNetCore.Mvc;
using MediatR;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISaleService _saleService;

        public SalesController(IMediator mediator, ISaleService saleService)
        {
            _mediator = mediator;
            _saleService = saleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetSaleResponse>>> GetAll()
        {
            var sales = await _saleService.GetAllAsync();
            var response = sales.Select(s => new GetSaleResponse
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
            });
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetSaleResponse>> GetById(Guid id)
        {
            var sale = await _saleService.GetByIdAsync(id);
            if (sale == null)
                return NotFound();

            return Ok(new GetSaleResponse
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber,
                ProductId = sale.ProductId,
                ProductName = sale.ProductName,
                Quantity = sale.Quantity,
                UnitPrice = sale.UnitPrice,
                DiscountPercentage = sale.DiscountPercentage,
                DiscountAmount = sale.DiscountAmount,
                ItemTotalAmount = sale.ItemTotalAmount,
                TotalAmount = sale.TotalAmount,
                SaleDate = sale.SaleDate,
                CustomerId = sale.CustomerId,
                CustomerName = sale.CustomerName,
                SellerId = sale.SellerId,
                SellerName = sale.SellerName,
                BranchId = sale.BranchId,
                BranchName = sale.BranchName,
                Status = sale.Status
            });
        }

        [HttpGet("customer/{customerId:guid}")]
        public async Task<ActionResult<IEnumerable<GetSaleResponse>>> GetByCustomer(Guid customerId)
        {
            var sales = await _saleService.GetByCustomerAsync(customerId);
            var response = sales.Select(s => new GetSaleResponse
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
            });
            return Ok(response);
        }

        [HttpGet("seller/{sellerId:guid}")]
        public async Task<ActionResult<IEnumerable<GetSaleResponse>>> GetBySeller(Guid sellerId)
        {
            var sales = await _saleService.GetBySellerAsync(sellerId);
            var response = sales.Select(s => new GetSaleResponse
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
            });
            return Ok(response);
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<GetSaleResponse>>> GetByDateRange(
            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var sales = await _saleService.GetByDateRangeAsync(startDate, endDate);
            var response = sales.Select(s => new GetSaleResponse
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
            });
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CreateSaleResponse>> Create(CreateSaleRequest request)
        {
            var command = new CreateSaleCommand
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice,
                CustomerId = request.CustomerId,
                SellerId = request.SellerId,
                BranchId = request.BranchId
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(new CreateSaleResponse
                {
                    Success = false,
                    Message = result.ErrorMessage ?? "Ocorreu um erro desconhecido"
                });

            return Ok(new CreateSaleResponse
            {
                Success = true,
                SaleId = result.SaleId,
                Message = "Venda criada com sucesso"
            });
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateSaleRequest request)
        {
            var command = new UpdateSaleCommand
            {
                SaleId = id,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(new { message = result.ErrorMessage ?? "Ocorreu um erro desconhecido" });

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<DeleteSaleResponse>> Delete(Guid id)
        {
            var result = await _saleService.DeleteAsync(id);

            if (!result)
                return NotFound(new DeleteSaleResponse
                {
                    Success = false,
                    Message = "Venda não encontrada"
                });

            return Ok(new DeleteSaleResponse
            {
                Success = true,
                Message = "Venda excluída com sucesso"
            });
        }
    }
}