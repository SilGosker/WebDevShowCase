namespace ShowCase.Backend.Endpoints.Plant.Update;

public class UpdatePlantResponse
{
    public string Name { get; set; } = string.Empty;
    public int Duration { get; set; }
    public int Id { get; set; }
    public string? Password { get; set; }
}