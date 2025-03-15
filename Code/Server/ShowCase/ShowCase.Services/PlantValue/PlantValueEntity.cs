using ShowCase.Services.Database;
using ShowCase.Services.Plants;

namespace ShowCase.Services.PlantValue;

public class PlantValueEntity : DbEntity
{
    public DateTime RecordedAt { get; set; }
    public bool PumpState { get; set; }
    public Plant? Plant { get; set; }
    public int PlantId { get; set; }
}