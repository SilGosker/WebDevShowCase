using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity.Data;
using ShowCase.Backend.Endpoints.Account.Login;

namespace ShowCase.Backend.Endpoints.Plant.State;

public class PlantStateEndpointTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public PlantStateEndpointTests(TestApplicationFactory factory)
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
    public async Task GetPlantState_WithoutAuthorizationToken_ReturnsUnAuthorized()
    {
        // Arrange
        var authorizationToken = _client.DefaultRequestHeaders.Authorization;
        _client.DefaultRequestHeaders.Authorization = null;
        var plantId = TestApplicationFactory.ConnectedPlantId;

        // Act
        var response = await _client.GetAsync($"/plants/state/{plantId}");
        _client.DefaultRequestHeaders.Authorization = authorizationToken;
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetPlantState_WhenPlantDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var plantId = -1;
        
        // Act
        var response = await _client.GetAsync($"/plants/state/{plantId}");
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetPlantState_WhenPlantIsConnected_ReturnsConnected()
    {
        // Arrange
        var plantId = TestApplicationFactory.ConnectedPlantId;

        // Act
        var response = await _client.GetAsync($"/plants/state/{plantId}");
        var plantState = await response.Content.ReadFromJsonAsync<PlantStateResponse>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(plantState);
        Assert.Equal("Connected", plantState!.State);
    }

    [Fact]
    public async Task GetPlantState_WhenPlantIsDisconnected_ReturnsDisconnected()
    {
        // Arrange
        var plantId = TestApplicationFactory.DisconnectedPlantId;
        
        // Act
        var response = await _client.GetAsync($"/plants/state/{plantId}");
        var plantState = await response.Content.ReadFromJsonAsync<PlantStateResponse>();
        
        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(plantState);
        Assert.Equal("Disconnected", plantState!.State);
    }
}