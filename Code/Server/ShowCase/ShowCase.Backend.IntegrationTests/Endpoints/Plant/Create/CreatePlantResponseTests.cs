namespace ShowCase.Backend.Endpoints.Plant.Create;

public class CreatePlantResponseTests
{
    [Fact]
    public void Properties_ShouldSetValues()
    {
        // Arrange
        var response = new CreatePlantResponse();

        // Act
        response.Name = "Name";
        response.Id = 1;
        response.Password = "Password";
        response.Duration = 2;

        // Assert
        Assert.Equal("Name", response.Name);
        Assert.Equal(1, response.Id);
        Assert.Equal("Password", response.Password);
        Assert.Equal(2, response.Duration);
    }
}