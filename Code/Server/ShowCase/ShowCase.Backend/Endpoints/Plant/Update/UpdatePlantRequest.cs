namespace ShowCase.Backend.Endpoints.Plant.Update;

public class UpdatePlantRequest
{
    public string Name { get; set; } = string.Empty;
    public int Duration { get; set; }
    public bool RegeneratePassword { get; set; }
}