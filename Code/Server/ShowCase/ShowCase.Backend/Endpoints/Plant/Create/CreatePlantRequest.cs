namespace ShowCase.Backend.Endpoints.Plant.Create;

public class CreatePlantRequest
{
    public string Name { get; set; } = string.Empty;
    public int Duration { get; set; }
}