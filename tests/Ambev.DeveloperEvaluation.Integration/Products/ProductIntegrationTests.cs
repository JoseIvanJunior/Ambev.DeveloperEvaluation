using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Integration;
using FluentAssertions;
using Xunit;
using Bogus;
using System.Collections.Generic;

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

    private CreateProductCommand GenerateValidCreateProductCommand()
    {
        return new CreateProductCommand
        {
            Name = _faker.Commerce.ProductName(),
            Description = _faker.Commerce.ProductDescription(),
            Price = _faker.Finance.Amount(1, 1000, 2),
            StockQuantity = _faker.Random.Int(0, 500)
        };
    }

    [Fact(DisplayName = "POST /api/products - deve criar um produto com sucesso.")]
    public async Task CreateProduct_ValidData_ReturnsCreatedWithId()
    {

        var command = GenerateValidCreateProductCommand();

        var response = await _client.PostAsJsonAsync("/api/products", command);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.Created, $"API retornada {response.StatusCode} com conteúdo: {errorContent}");
        }

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseWithData<CreateProductResult>>();
        var result = apiResponse?.Data;

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        result.Should().NotBeNull();
        result!.Id.Should().NotBe(Guid.Empty);
    }

    [Fact(DisplayName = "POST /api/products - deve falhar com dados inválidos.")]
    public async Task CreateProduct_InvalidData_ReturnsBadRequest()
    {

        var invalidCommand = new CreateProductCommand
        {
            Name = "",
            Description = _faker.Lorem.Sentence(101),
            Price = -10.0m,
            StockQuantity = -5
        };

        var response = await _client.PostAsJsonAsync("/api/products", invalidCommand);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.InternalServerError);
    }

    [Fact(DisplayName = "GET /api/products/{id} - deve retornar 404 ao buscar um produto inexistente.")]
    public async Task GetProductById_NotFound_ReturnsNotFound()
    {

        var nonExistentProductId = Guid.NewGuid();

        var response = await _client.GetAsync($"/api/products/{nonExistentProductId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "GET /api/products/{id} - deve retornar produto existente com sucesso.")]
    public async Task GetProductById_Found_ReturnsOkWithData()
    {

        var createCommand = GenerateValidCreateProductCommand();
        var createResponse = await _client.PostAsJsonAsync("/api/products", createCommand);
        createResponse.EnsureSuccessStatusCode();
        var createdApiResponse = await createResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateProductResult>>();
        var createdProductId = createdApiResponse!.Data!.Id;

        var getResponse = await _client.GetAsync($"/api/products/{createdProductId}");

        if (!getResponse.IsSuccessStatusCode)
        {
            var errorContent = await getResponse.Content.ReadAsStringAsync();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK, $"API retornada {getResponse.StatusCode} com conteúdo: {errorContent}");
        }

        var getApiResponse = await getResponse.Content.ReadFromJsonAsync<ApiResponseWithData<ApiResponseWithData<GetProductResult>>>();
        var product = getApiResponse?.Data?.Data;

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        product.Should().NotBeNull();
        product!.Id.Should().Be(createdProductId);
        product.Name.Should().Be(createCommand.Name);
        product.Price.Should().Be(createCommand.Price);
    }

    [Fact(DisplayName = "GET /api/products - deve retornar lista de produtos.")]
    public async Task GetAllProducts_ReturnsOkWithList()
    {

        var createCommand = GenerateValidCreateProductCommand();
        var createResponse = await _client.PostAsJsonAsync("/api/products", createCommand);
        createResponse.EnsureSuccessStatusCode();

        var response = await _client.GetAsync("/api/products");

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK, $"API retornada {response.StatusCode} com conteúdo: {errorContent}");
        }

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseWithData<ApiResponseWithData<IEnumerable<GetProductResult>>>>();
        var products = apiResponse?.Data?.Data;

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        products.Should().NotBeNull();
        products.Should().NotBeEmpty();
    }

    [Fact(DisplayName = "PUT /api/products/{id} - deve atualizar um produto com sucesso.")]
    public async Task UpdateProduct_ValidData_ReturnsOk()
    {
        var createCommand = GenerateValidCreateProductCommand();
        var createResponse = await _client.PostAsJsonAsync("/api/products", createCommand);
        createResponse.EnsureSuccessStatusCode();
        var createdApiResponse = await createResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateProductResult>>();
        var productId = createdApiResponse!.Data!.Id;

        var updateCommand = new UpdateProductCommand
        {
            Name = _faker.Commerce.ProductName(),
            Description = _faker.Commerce.ProductDescription(),
            Price = _faker.Finance.Amount(1, 1000, 2),
            StockQuantity = _faker.Random.Int(0, 500)
        };

        var response = await _client.PutAsJsonAsync($"/api/products/{productId}", updateCommand);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact(DisplayName = "DELETE /api/products/{id} - deve deletar um produto com sucesso.")]
    public async Task DeleteProduct_ExistingId_ReturnsNoContent()
    {
        var createCommand = GenerateValidCreateProductCommand();
        var createResponse = await _client.PostAsJsonAsync("/api/products", createCommand);
        createResponse.EnsureSuccessStatusCode();
        var createdApiResponse = await createResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateProductResult>>();
        var productId = createdApiResponse!.Data!.Id;

        var response = await _client.DeleteAsync($"/api/products/{productId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}