using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.DeveloperEvaluation.Integration;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentAssertions;
using Xunit;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Users;

public class UserIntegrationTests : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client;
    private readonly Faker _faker;

    public UserIntegrationTests(WebApiFactory factory)
    {
        _client = factory.CreateClient();
        _faker = new Faker("pt_BR");
    }

    [Fact(DisplayName = "POST /api/users - deve criar um usuário com sucesso.")]
    public async Task CreateUser_ReturnsSuccess()
    {
        var request = new CreateUserRequest
        {
            Username = _faker.Internet.UserName(),
            Password = $"P@ss{_faker.Internet.DomainWord()}{_faker.Random.Number(10, 99)}!",
            Email = _faker.Internet.Email(provider: "test.ambev.com"),
            Phone = "+556413480521",
            Status = UserStatus.Active,
            Role = UserRole.Customer
        };

        var response = await _client.PostAsJsonAsync("/api/users", request);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.Created,
                $"API retornada {response.StatusCode} com conteúdo: {errorContent}");
        }

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseWithData<CreateUserResponse>>();
        var result = apiResponse?.Data;

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Should().NotBeNull();
        result!.Id.Should().NotBe(Guid.Empty);
    }

    [Fact(DisplayName = "GET /api/users/{id} - deve retornar 404 ao buscar um usuário inexistente.")]
    public async Task GetUserById_ReturnsNotFound()
    {
        var nonExistentUserId = Guid.NewGuid();
        var response = await _client.GetAsync($"/api/users/{nonExistentUserId}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "POST /api/users - deve falhar com dados inválidos.")]
    public async Task CreateUser_InvalidData_ReturnsBadRequest()
    {
        var invalidRequest = new CreateUserRequest
        {
            Username = "",
            Password = "123",
            Email = "invalid-email",
            Phone = "12345",
            Status = (UserStatus)999,
            Role = (UserRole)999
        };

        var response = await _client.PostAsJsonAsync("/api/users", invalidRequest);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}