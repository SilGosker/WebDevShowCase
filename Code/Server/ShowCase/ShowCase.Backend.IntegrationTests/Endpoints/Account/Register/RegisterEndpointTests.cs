using System.Net;
using System.Text;
using System.Text.Json;

namespace ShowCase.Backend.Endpoints.Account.Register;

public class RegisterEndpointTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public RegisterEndpointTests(TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task RegisterEndpoint_WithEmptyEmail_ReturnsError()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/account/register")
        {
            Content = new StringContent(JsonSerializer.Serialize(new RegisterRequest
            {
                Email = "",
                Password = "password"
            }), Encoding.UTF8, "application/json")
        };
        
        // Act
        var response = await _client.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("email\":", json);
        Assert.Contains("not be empty", json);
    }

    [Fact]
    public async Task RegisterEndpoint_WithInvalidEmail_ReturnsError()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/account/register")
        {
            Content = new StringContent(JsonSerializer.Serialize(new RegisterRequest
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
        Assert.Contains("email\":", json);
        Assert.Contains("not a valid email", json);
    }

    [Fact]
    public async Task RegisterEndpoint_WithEmptyPassword_ReturnsError()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/account/register")
        {
            Content = new StringContent(JsonSerializer.Serialize(new RegisterRequest
            {
                Email = "someone@example.com",
                Password = ""
            }), Encoding.UTF8, "application/json")
        };

        // Act
        var response = await _client.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("password\":", json);
        Assert.Contains("not be empty", json);
    }

    [Fact]
    public async Task RegisterEndpoint_WithShortPassword_ReturnsError()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/account/register")
        {
            Content = new StringContent(JsonSerializer.Serialize(new RegisterRequest
            {
                Email = "someone@example.com",
                Password = "dd"
            }), Encoding.UTF8, "application/json")
        };

        // Act
        var response = await _client.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("password\":", json);
        Assert.Contains("be at least 8", json);
    }

    [Fact]
    public async Task RegisterEndpoint_WithLongPassword_ReturnsError()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/account/register")
        {
            Content = new StringContent(JsonSerializer.Serialize(new RegisterRequest
            {
                Email = "someone@example.com",
                Password = "d".PadLeft(65)
            }), Encoding.UTF8, "application/json")
        };

        // Act
        var response = await _client.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("password\":", json);
        Assert.Contains("must be 64", json);
    }

    [Fact]
    public async Task RegisterEndpoint_WhenEmailExists_Returns400BadRequest()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/account/register")
        {
            Content = new StringContent(JsonSerializer.Serialize(new RegisterRequest
            {
                Email = "existing@example.com",
                Password = "password"
            }), Encoding.UTF8, "application/json")
        };

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task RegisterEndpoint_WhenEmailDoesNotExist_Returns200Ok()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/account/register")
        {
            Content = new StringContent(JsonSerializer.Serialize(new RegisterRequest
            {
                Email = "not-existingmail@example.com",
                Password = "password"
            }), Encoding.UTF8, "application/json")
        };

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}