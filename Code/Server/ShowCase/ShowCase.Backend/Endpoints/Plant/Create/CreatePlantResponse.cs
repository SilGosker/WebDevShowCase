namespace ShowCase.Backend.Endpoints.Plant.Create;

public class CreatePlantResponse
{
    public string Name { get; set; } = string.Empty;
    public int Duration { get; set; }
    public string Password { get; set; } = string.Empty;
    public int Id { get; set; }
}