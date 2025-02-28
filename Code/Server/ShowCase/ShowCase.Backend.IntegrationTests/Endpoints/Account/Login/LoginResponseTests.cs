namespace ShowCase.Backend.Endpoints.Account.Login;

public class LoginResponseTests
{
    [Fact]
    public void Properties_ShouldSetValues()
    {
        // Arrange
        var token = "test";
        var role = "user";
        var response = new LoginResponse();

        // Act
        response.Token = token;
        response.Role = role;

        // Assert
        Assert.Equal(token, response.Token);
        Assert.Equal(role, response.Role);
    }
}