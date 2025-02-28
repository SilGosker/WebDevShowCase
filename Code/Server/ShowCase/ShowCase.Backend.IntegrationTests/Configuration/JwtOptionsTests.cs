namespace ShowCase.Backend.Configuration;

public class JwtOptionsTests
{
    [Fact]
    public void Properties_ShouldSetProperties()
    {
        // Arrange
        var options = new JwtOptions();

        // Act
        options.SecretKey = "Secret";
        options.Issuer = "Issuer";
        options.Audience = "Audience";

        // Assert
        Assert.Equal("Secret", options.SecretKey);
        Assert.Equal("Issuer", options.Issuer);
        Assert.Equal("Audience", options.Audience);
    }
}