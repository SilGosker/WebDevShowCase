namespace ShowCase.Backend.Endpoints.Plant.Index;

public class PlantResponseTests 
{
    [Fact]
    public void Properties_ShouldSetValues()
    {
        // Arrange
        var plant = new PlantResponse();


        // Act
        plant.Id = 1;
        plant.Name = "Plant 1";
     
        // Assert
        Assert.Equal(1, plant.Id);
        Assert.Equal("Plant 1", plant.Name);
    }
}