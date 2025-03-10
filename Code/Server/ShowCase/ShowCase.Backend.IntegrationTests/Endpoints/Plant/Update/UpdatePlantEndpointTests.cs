using System.Net;
using ShowCase.Backend.Endpoints.Account.Login;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ShowCase.Backend.Endpoints.Plant.Update;

public class UpdatePlantEndpointTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public UpdatePlantEndpointTests(TestApplicationFactory factory)
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
    public async Task UpdatePlantEndpoint_WhenUnAuthorized_ReturnsUnAuthorized()
    {
        // Arrange
        var authorizationToken = _client.DefaultRequestHeaders.Authorization;
        _client.DefaultRequestHeaders.Authorization = null;
        var request = new UpdatePlantRequest
        {
            Name = "MyPlant",
            Duration = 30
        };

        // Act
        var response = await _client.PutAsJsonAsync("/plants/update/1", request);
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task UpdatePlantEndpoint_WhenValidRequest_ReturnsSuccess()
    {
        // Arrange
        var request = new UpdatePlantRequest
        {
            Name = "MyPlant",
            Duration = 30,
            RegeneratePassword = false
        };
        
        // Act
        var response = await _client.PutAsJsonAsync("/plants/update/1", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseData = await response.Content.ReadFromJsonAsync<UpdatePlantResponse>();
        Assert.NotNull(responseData);
        Assert.Equal(request.Name, responseData.Name);
        Assert.Equal(request.Duration, responseData.Duration);
        Assert.True(string.IsNullOrEmpty(responseData.Password));
    }
}