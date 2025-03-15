using ShowCase.Services.PlantValue;

namespace ShowCase.Backend.Endpoints.Plant.Details;

public class PlantDetailsResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Duration { get; set; }
    public PlantValuesResponse[] PlantValues { get; set; } = Array.Empty<PlantValuesResponse>();
}

public class PlantValuesResponse
{
    public DateTime RecordedAt { get; set; }
    public bool PumpState { get; set; }
}