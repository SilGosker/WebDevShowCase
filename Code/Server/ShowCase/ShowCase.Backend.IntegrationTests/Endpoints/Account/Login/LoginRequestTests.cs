namespace ShowCase.Backend.Endpoints.Account.Login;

public class LoginRequestTests
{
    [Fact]
    public void Properties_ShouldSetValues()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password";
        var request = new LoginRequest();

        // Act
        request.Email = email;
        request.Password = password;

        // Assert
        Assert.Equal(email, request.Email);
        Assert.Equal(password, request.Password);
    }
}