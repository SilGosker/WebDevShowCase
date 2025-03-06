namespace ShowCase.Backend.Endpoints.Plant.Create;

public class CreatePlantRequestTests
{
    [Fact]
    public void Properties_ShouldSetValues()
    {
        // Arrange
        var request = new CreatePlantRequest();

        // Act
        request.Name = "Name";
        request.Duration = 1;

        // Assert
        Assert.Equal("Name", request.Name);
        Assert.Equal(1, request.Duration);
    }
}