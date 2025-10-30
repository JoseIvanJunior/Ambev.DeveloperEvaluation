using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Integration;
using FluentAssertions;
using Xunit;
using Bogus;
using NSubstitute;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Integration.Products;

public class ProductIntegrationTests : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client;
    private readonly Faker _faker;

    public ProductIntegrationTests(WebApiFactory factory)
    {
        _client = factory.CreateClient();
        _faker = new Faker("pt_BR");
    }

    [Fact(DisplayName = "POST /api/products - deve criar um produto com sucesso")]
    public async Task CreateProduct_ValidData_ReturnsCreated()
    {
        
        var productRequest = new
        {
            Name = _faker.Commerce.ProductName(),
            Description = _faker.Commerce.ProductDescription(),
            Price = _faker.Finance.Amount(1, 1000, 2),
            StockQuantity = _faker.Random.Int(0, 500)
        };

        var response = await _client.PostAsJsonAsync("/api/products", productRequest);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseWithData<object>>();
        apiResponse.Should().NotBeNull();
        apiResponse!.Success.Should().BeTrue();
    }

    [Fact(DisplayName = "POST /api/products - deve falhar com dados inválidos")]
    public async Task CreateProduct_InvalidData_ReturnsBadRequest()
    {
        
        var invalidRequest = new
        {
            Name = "",
            Description = "Desc",
            Price = -10.0m,
            StockQuantity = -5
        };

        var response = await _client.PostAsJsonAsync("/api/products", invalidRequest);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "GET /api/products/{id} - deve retornar 404 para produto inexistente")]
    public async Task GetProductById_NonExistent_ReturnsNotFound()
    {
        
        var response = await _client.GetAsync($"/api/products/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "GET /api/products - deve retornar lista de produtos")]
    public async Task GetAllProducts_ReturnsOkWithProducts()
    {
        
        var response = await _client.GetAsync("/api/products");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();

        // Usar um tipo mais simples para desserialização
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseWithData<object>>();
        apiResponse.Should().NotBeNull();
        apiResponse!.Success.Should().BeTrue();
        apiResponse.Data.Should().NotBeNull();
    }
}