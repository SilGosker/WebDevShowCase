namespace ShowCase.Backend.Endpoints.Plant.Update;

public class UpdatePlantResponseTests
{
    [Fact]
    public void Properties_ShouldSetValues()
    {
        // Arrange
        var response = new UpdatePlantResponse();

        // Act
        response.Name = "Name";
        response.Duration = int.MaxValue;
        response.Id = int.MinValue;
        response.Password = "Password";

        // Assert
        Assert.Equal("Name", response.Name);
        Assert.Equal(int.MaxValue, response.Duration);
        Assert.Equal(int.MinValue, response.Id);
        Assert.Equal("Password", response.Password);
    }
}