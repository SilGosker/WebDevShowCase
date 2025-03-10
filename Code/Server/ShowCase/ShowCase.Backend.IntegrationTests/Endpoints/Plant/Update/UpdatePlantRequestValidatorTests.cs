namespace ShowCase.Backend.Endpoints.Plant.Update;

public class UpdatePlantRequestValidatorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WhenNameIsEmpty_ShouldReturnInvalidResult(string? name)
    {
        // Arrange
        var request = new UpdatePlantRequest
        {
            Name = name!,
        };

        // Act
        var result = new UpdatePlantRequestValidator().Validate(request);
        
        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(request.Name));
    }

    [Fact]
    public void Validate_WhenNameIsLongerThan250Chars_ShouldReturnInvalidResult()
    {
        // Arrange
        var request = new UpdatePlantRequest
        {
            Name = new string('a', 251),
        };
        
        // Act
        var result = new UpdatePlantRequestValidator().Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(request.Name));
    }

    [Fact]
    public void Validate_WhenDurationIsLessThanOne_ShouldReturnInvalidResult()
    {
        // Arrange
        var request = new UpdatePlantRequest
        {
            Duration = 0,
        };
        // Act
        var result = new UpdatePlantRequestValidator().Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(request.Duration));
    }

    [Fact]
    public void Validate_WhenDurationIsGreaterThan300_ShouldReturnInvalidResult()
    {
        // Arrange
        var request = new UpdatePlantRequest
        {
            Duration = 301,
        };
        // Act
        var result = new UpdatePlantRequestValidator().Validate(request);
        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(request.Duration));
    }

    [Fact]
    public void Validate_WhenRequestIsValid_ShouldReturnValidResult()
    {
        // Arrange
        var request = new UpdatePlantRequest
        {
            Name = "Name",
            Duration = 300,
        };

        // Act
        var result = new UpdatePlantRequestValidator().Validate(request);
        
        // Assert
        Assert.True(result.IsValid);
    }
}