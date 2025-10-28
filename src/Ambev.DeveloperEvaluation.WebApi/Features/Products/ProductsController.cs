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

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateProductResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct(
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = _mapper.Map<CreateProductCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);
        var response = _mapper.Map<CreateProductResponse>(result);

        return CreatedAtAction(
            nameof(GetProductById),
            new { id = result.Id },
            new ApiResponseWithData<CreateProductResponse>
            {
                Success = true,
                Data = response,
                Message = "Produtos criados com sucesso"
            });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(
        Guid id,
        CancellationToken cancellationToken = default)
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
            Message = "Produtos recuperados com sucesso"
        });
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<GetProductResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProducts(
        CancellationToken cancellationToken = default)
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

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(
        Guid id,
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken = default)
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

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(
        Guid id,
        CancellationToken cancellationToken = default)
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
}