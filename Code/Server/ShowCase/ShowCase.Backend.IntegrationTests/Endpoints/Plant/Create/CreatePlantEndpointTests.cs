using ShowCase.Backend.Endpoints.Account.Login;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ShowCase.Backend.Endpoints.Plant.Create;

public class CreatePlantEndpointTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public CreatePlantEndpointTests(TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
        AddAuthenticationToken().Wait();
    }


    private async Task AddAuthenticationToken()
    {
        var response = await _client.PostAsJsonAsync("/account/login", new
        {
            Email = "existing@example.com",
            Password = "correct-password"
        });
        response.EnsureSuccessStatusCode();
        var token = await response.Content.ReadFromJsonAsync<LoginResponse>();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token!.Token);
    }

    [Fact]
    public async Task CreatePlantEndpoint_WhenValidRequest_ReturnsSuccess()
    {
        // Arrange
        var request = new CreatePlantRequest
        {
            Name = "MyPlant",
            Duration = 30
        };

        // Act
        var response = await _client.PostAsJsonAsync("/plants/create", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseData = await response.Content.ReadFromJsonAsync<CreatePlantResponse>();
        Assert.NotNull(responseData);
        Assert.Equal(request.Name, responseData.Name);
        Assert.Equal(request.Duration, responseData.Duration);
        Assert.False(string.IsNullOrEmpty(responseData.Password));
    }

    [Fact]
    public async Task CreatePlantEndpoint_WhenUnauthorized_ReturnsUnauthorized()
    {
        // Arrange
        var authorizationToken = _client.DefaultRequestHeaders.Authorization;
        _client.DefaultRequestHeaders.Authorization = null;
        var request = new CreatePlantRequest
        {
            Name = "UnauthorizedPlant",
            Duration = 30
        };

        // Act
        var response = await _client.PostAsJsonAsync("/plants/create", request);
        _client.DefaultRequestHeaders.Authorization = authorizationToken;

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
