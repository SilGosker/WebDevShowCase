using ShowCase.Backend.Endpoints.Account.Login;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http.Json;

namespace ShowCase.Backend.Endpoints.Plant.Index;

public class PlantIndexEndpointTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public PlantIndexEndpointTests(TestApplicationFactory factory)
    {
        this._client = factory.CreateClient();
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
    public async Task GetPlants_WithoutAuthorizationToken_ReturnsUnAuthorized()
    {
        // Arrange
        var authorizationToken = _client.DefaultRequestHeaders.Authorization;
        _client.DefaultRequestHeaders.Authorization = null;

        // Act
        var response = await _client.GetAsync("/plants");
        _client.DefaultRequestHeaders.Authorization = authorizationToken;

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetPlants_WhenUserHasPlants_ReturnsPlants()
    {
        // Act
        var response = await _client.GetAsync("/plants");

        // Assert
        response.EnsureSuccessStatusCode();
        var plants = await response.Content.ReadFromJsonAsync<List<PlantResponseTests>>();
        Assert.NotNull(plants);
        Assert.Empty(plants);
    }
}