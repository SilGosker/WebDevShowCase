namespace ShowCase.Backend.Endpoints.Plant.Details;

public class PlantDetailsResponseTests
{
    [Fact]
    public void Properties_ShouldSetValues()
    {
        // Arrange
        var plantDetailsResponse = new PlantDetailsResponse();

        // Act
        plantDetailsResponse.Id = 1;
        plantDetailsResponse.Name = "Plant 1";
        
        // Assert
        Assert.Equal(1, plantDetailsResponse.Id);
        Assert.Equal("Plant 1", plantDetailsResponse.Name);
    }


}