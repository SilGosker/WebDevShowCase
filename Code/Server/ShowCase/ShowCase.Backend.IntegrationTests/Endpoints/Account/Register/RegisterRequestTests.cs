namespace ShowCase.Backend.Endpoints.Account.Register;

public class RegisterRequestTests
{
    [Fact]
    public void Properties_ShouldSetValues()
    {
        // Arrange
        var request = new RegisterRequest();

        // Act
        request.Email = "example@mail.com";
        request.Password = "password";

        // Assert
        Assert.Equal("example@mail.com", request.Email);
        Assert.Equal("password", request.Password);
    }
}