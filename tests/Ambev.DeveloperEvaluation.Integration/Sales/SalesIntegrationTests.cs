using Xunit;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Integration;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Common.Responses;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Ambev.DeveloperEvaluation.Integration.Sales
{
    public class SalesIntegrationTests : IClassFixture<WebApiFactory>
    {
        private readonly HttpClient _client;

        public SalesIntegrationTests(WebApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateSale_WithValidData_ReturnsCreated()
        {
            var request = new CreateSaleRequest
            {
                ProductId = Guid.NewGuid(),
                Quantity = 2,
                UnitPrice = 25.50m,
                CustomerId = Guid.NewGuid(),
                SellerId = Guid.NewGuid(),
                BranchId = Guid.NewGuid()
            };

            var response = await _client.PostAsJsonAsync("/api/sales", request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<CreateSaleResponse>>();
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.SaleId.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CreateSale_WithInvalidQuantity_ReturnsBadRequest()
        {
            var request = new CreateSaleRequest
            {
                ProductId = Guid.NewGuid(),
                Quantity = 0,
                UnitPrice = 25.50m,
                CustomerId = Guid.NewGuid(),
                SellerId = Guid.NewGuid(),
                BranchId = Guid.NewGuid()
            };

            var response = await _client.PostAsJsonAsync("/api/sales", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetAllSales_ReturnsOkWithSales()
        {
            var response = await _client.GetAsync("/api/sales");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<object>>>();
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
        }

        [Fact]
        public async Task GetSaleById_WithNonExistingId_ReturnsNotFound()
        {
            var nonExistingId = Guid.NewGuid();

            var response = await _client.GetAsync($"/api/sales/{nonExistingId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}