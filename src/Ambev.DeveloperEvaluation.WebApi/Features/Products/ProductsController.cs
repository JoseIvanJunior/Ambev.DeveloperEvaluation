using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Common;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IMediator mediator, IMapper mapper, ILogger<ProductsController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateProductResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var command = _mapper.Map<CreateProductCommand>(request);
            var result = await _mediator.Send(command, cancellationToken);

            var response = _mapper.Map<CreateProductResponse>(result);

            var apiResponse = new ApiResponseWithData<CreateProductResponse>
            {
                Success = true,
                Message = "Produto criado com sucesso",
                Data = response
            };

            return CreatedAtAction(nameof(GetProductById), new { id = response.Id }, apiResponse);
        }
        catch (ValidationException ex)
        {
            // Remover a propriedade Error que não existe
            var apiResponse = new ApiResponse
            {
                Success = false,
                Message = $"Dados inválidos: {string.Join("; ", ex.Errors.Select(e => e.ErrorMessage))}"
            };
            return BadRequest(apiResponse);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Já existe um produto"))
        {
            var apiResponse = new ApiResponse
            {
                Success = false,
                Message = ex.Message
            };
            return BadRequest(apiResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar produto");

            var apiResponse = new ApiResponse
            {
                Success = false,
                Message = "Ocorreu um erro inesperado"
            };
            return StatusCode(500, apiResponse);
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetProductQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);

            if (result == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Produto não encontrado"
                });
            }

            var response = _mapper.Map<GetProductResponse>(result);

            return Ok(new ApiResponseWithData<GetProductResponse>
            {
                Success = true,
                Data = response,
                Message = "Produto recuperado com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar produto por ID: {ProductId}", id);
            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "Ocorreu um erro inesperado"
            });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<GetProductResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProducts(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetProductsQuery();
            var result = await _mediator.Send(query, cancellationToken);
            var response = _mapper.Map<IEnumerable<GetProductResponse>>(result);

            return Ok(new ApiResponseWithData<IEnumerable<GetProductResponse>>
            {
                Success = true,
                Data = response,
                Message = "Produtos recuperados com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar todos os produtos");
            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "Ocorreu um erro inesperado"
            });
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(
        Guid id,
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = _mapper.Map<UpdateProductCommand>(request);
            command.Id = id;

            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<UpdateProductResponse>(result);

            return Ok(new ApiResponseWithData<UpdateProductResponse>
            {
                Success = true,
                Data = response,
                Message = "Produto atualizado com sucesso"
            });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "Produto não encontrado"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar produto: {ProductId}", id);
            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "Ocorreu um erro inesperado"
            });
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteProductCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);

            if (!result)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Produto não encontrado"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Produto excluído com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir produto: {ProductId}", id);
            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "Ocorreu um erro inesperado"
            });
        }
    }
}