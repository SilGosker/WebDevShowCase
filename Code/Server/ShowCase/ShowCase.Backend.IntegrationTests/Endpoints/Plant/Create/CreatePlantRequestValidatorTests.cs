namespace ShowCase.Backend.Endpoints.Plant.Create;

public class CreatePlantRequestValidatorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WhenNameIsEmpty_ReturnsInvalidResult(string? name)
    {
        // Arrange
        var request = new CreatePlantRequest
        {
            Name = name!,
            Duration = 1
        };
        var validator = new CreatePlantRequestValidator();
        
        // Act
        var result = validator.Validate(request);
        
        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(CreatePlantRequest.Name));
    }

    [Fact]
    public void Validate_WhenNameIsLongerThan300Chars_ReturnsInvalidResult()
    {
        // Arrange
        var request = new CreatePlantRequest
        {
            Name = new string('a', 301),
            Duration = 1
        };
        var validator = new CreatePlantRequestValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(CreatePlantRequest.Name));
    }

    [Fact]
    public void Validate_WhenDurationIsLessThanOne_ReturnsInvalidResult()
    {
        // Arrange
        var request = new CreatePlantRequest
        {
            Name = "Name",
            Duration = 0
        };
        var validator = new CreatePlantRequestValidator();

        // Act
        var result = validator.Validate(request);
        
        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(CreatePlantRequest.Duration));
    }

    [Fact]
    public void Validate_WhenDurationIsMoreThan300_ReturnsInvalidResult()
    {
        // Arrange
        var request = new CreatePlantRequest
        {
            Name = "Name",
            Duration = 301
        };
        var validator = new CreatePlantRequestValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == nameof(CreatePlantRequest.Duration));
    }

    [Fact]
    public void Validate_WhenRequestIsValid_ReturnsValidResult()
    {
        // Arrange
        var request = new CreatePlantRequest
        {
            Name = "Name",
            Duration = 1
        };
        var validator = new CreatePlantRequestValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }
}