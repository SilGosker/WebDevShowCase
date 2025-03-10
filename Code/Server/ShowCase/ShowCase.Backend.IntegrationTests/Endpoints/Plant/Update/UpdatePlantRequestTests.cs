namespace ShowCase.Backend.Endpoints.Plant.Update;

public class UpdatePlantRequestTests
{
    [Fact]
    public void Properties_ShouldSetValues()
    {
        // Arrange
        var request = new UpdatePlantRequest();

        // Act
        request.Name = "Name";
        request.Duration= int.MaxValue;
        request.RegeneratePassword= false;

        // Assert
        Assert.Equal("Name", request.Name);
        Assert.Equal(int.MaxValue, request.Duration);
        Assert.False(request.RegeneratePassword);
    }
}