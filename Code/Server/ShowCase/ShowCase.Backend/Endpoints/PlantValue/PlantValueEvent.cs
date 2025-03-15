using EasySockets.Events;

namespace ShowCase.Backend.Endpoints.PlantValue;

public class PlantValueEvent : IEasySocketEvent
{
    public PlantValueEvent(char pumpState)
    {
        PumpState = pumpState == '1';
    }
    public string Event { get; set; } = string.Empty;
    public bool PumpState { get; set; }
}