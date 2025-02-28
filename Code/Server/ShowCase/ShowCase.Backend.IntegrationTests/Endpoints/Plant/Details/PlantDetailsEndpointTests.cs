using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ShowCase.Backend.Endpoints.Account.Login;

namespace ShowCase.Backend.Endpoints.Plant.Details;

public class PlantDetailsEndpointTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public PlantDetailsEndpointTests(TestApplicationFactory factory)
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
    public async Task GetPlantDetails_WithoutAuthorizationToken_ReturnsUnAuthorized()
    {
        // Arrange
        var authorizationToken = _client.DefaultRequestHeaders.Authorization;
        var plantId = TestApplicationFactory.ConnectedPlantId;
        _client.DefaultRequestHeaders.Authorization = null;

        // Act
        var response = await _client.GetAsync($"/plants/{plantId}");
        _client.DefaultRequestHeaders.Authorization = authorizationToken;

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetPlantDetails_WhenPlantIsFound_ReturnsPlantDetails()
    {
        // Arrange
        var plantId = TestApplicationFactory.ConnectedPlantId;

        // Act
        var response = await _client.GetAsync($"/plants/{plantId}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var plantDetails = await response.Content.ReadFromJsonAsync<PlantDetailsResponse>();

        Assert.NotNull(plantDetails);
        Assert.Equal(plantId, plantDetails.Id);
    }

    [Fact]
    public async Task GetPlantDetails_WhenPlantIsNotFound_ReturnsPlantDetails()
    {
        // Arrange
        var plantId = -1;

        // Act
        var response = await _client.GetAsync($"/plants/{plantId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}