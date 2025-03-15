using EasySockets.Events;

namespace ShowCase.Backend.Endpoints.PlantValue;

public abstract class PlantValueSocket : EventSocket<PlantValueEvent>
{
    public override PlantValueEvent? ExtractEvent(string message)
    {
        ReadOnlySpan<char> span = message.AsSpan();
        if (span.Length < 2)
        {
            return null;
        }

        if (span[0] != 'p')
        {
            return null;
        }

        return new PlantValueEvent(span[2])
        {
            Event = "p"
        };
    }

    public override string? BindEvent(string @event, string message)
    {
        return $"{@event}:{message}";
    }
}