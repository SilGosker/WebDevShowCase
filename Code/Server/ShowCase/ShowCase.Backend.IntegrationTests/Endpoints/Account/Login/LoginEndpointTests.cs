using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ShowCase.Backend.Endpoints.Account.Login;

public class LoginEndpointTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public LoginEndpointTests(TestApplicationFactory applicationFactory)
    {
        _client = applicationFactory.CreateClient();
    }

    [Fact]
    public async Task Login_WithInvalidEmail_ReturnsError()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/account/login")
        {
            Content = new StringContent(JsonSerializer.Serialize(new LoginRequest
            {
                Email = "invalid-email",
                Password = "password"
            }), Encoding.UTF8, "application/json")
        };

        // Act
        var response = await _client.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("email", json);
    }

    [Fact]
    public async Task Login_WithGetMethod_Returns415StatusCode()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/account/login");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithNonExistingEmail_Returns400StatusCode()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/account/login")
        {
            Content = new StringContent(JsonSerializer.Serialize(new LoginRequest
            {
                Email = "non-existing-email@non-existing.com"
            }), Encoding.UTF8, "application/json")
        };

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithInvalidPassword_Returns400StatusCode()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/account/login")
        {
            Content = new StringContent(JsonSerializer.Serialize(new LoginRequest
            {
                Email = "existing@example.com",
                Password = "invalid-password"
            }), Encoding.UTF8, "application/json")
        };

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithValidCredentials_Returns200StatusCode()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/account/login")
        {
            Content = new StringContent(JsonSerializer.Serialize(new LoginRequest
            {
                Email = "existing@example.com",
                Password = "correct-password"
            }), Encoding.UTF8, "application/json")
        };

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsTokenAndRole()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/account/login")
        {
            Content = new StringContent(JsonSerializer.Serialize(new LoginRequest
            {
                Email = "existing@example.com",
                Password = "correct-password"
            }, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }), Encoding.UTF8, "application/json")
        };

        // Act
        var response = await _client.SendAsync(request);
        var tokenResponse = await response.Content.ReadFromJsonAsync<LoginResponse>()!;

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(tokenResponse!.Token);
        Assert.NotNull(tokenResponse.Role);
    }
}