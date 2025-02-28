namespace ShowCase.Backend.Endpoints.Plant.State;

public class PlantStateRequestTests
{
    [Fact]
    public void Properties_ShouldSetValues()
    {
        // Arrange
        var plantStateRequest = new PlantStateRequest();
        
        // Act
        plantStateRequest.Id = 1;
        
        // Assert
        Assert.Equal(1, plantStateRequest.Id);
    }
}