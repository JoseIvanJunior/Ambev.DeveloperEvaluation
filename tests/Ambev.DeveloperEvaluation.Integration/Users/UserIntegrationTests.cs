using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Users;

public class UserIntegrationTests : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client;

    public UserIntegrationTests(WebApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact(DisplayName = "POST /api/users - deve criar um usuário com sucesso.")]
    public async Task CreateUser_ReturnsSuccess()
    {
        var command = new CreateUserCommand // Ou use CreateUserRequest se o controller espera isso
        {
            Username = "teste.integration",
            Password = "Test@12345",
            Email = $"integration_{Guid.NewGuid()}@ambev.com",
            Phone = "+5511999999999",
            Status = UserStatus.Active,
            Role = UserRole.Customer
        };

        var response = await _client.PostAsJsonAsync("/api/users", command); // Envia CreateUserCommand ou CreateUserRequest

        // 👇👇👇 CORREÇÃO AQUI 👇👇👇
        // Leia como ApiResponseWithData<T> e pegue o .Data
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseWithData<CreateUserResponse>>();
        var result = apiResponse?.Data;
        // 👆👆👆 CORREÇÃO AQUI 👆👆👆

        response.StatusCode.Should().Be(HttpStatusCode.Created); // Status 201 é o correto aqui
        result.Should().NotBeNull();
        result!.Id.Should().NotBe(Guid.Empty); // Agora deve funcionar
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
        var invalidCommand = new CreateUserCommand
        {
            Username = "",
            Password = "123",
            Email = "invalid-email",
            Phone = "12345",
            Status = (UserStatus)999,
            Role = (UserRole)999
        };

        var response = await _client.PostAsJsonAsync("/api/users", invalidCommand);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
