namespace ShowCase.Backend.Endpoints.Plant.State;

public class PlantStateResponseTests
{
    [Fact]
    public void Properties_ShouldSetValues()
    {
        // Arrange
        var plantState = new PlantStateResponse();

        // Act
        plantState.State = "State";

        // Assert
        Assert.Equal("State", plantState.State);
    }
}