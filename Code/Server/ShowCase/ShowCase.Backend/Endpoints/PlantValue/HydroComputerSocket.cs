using EasySockets.Attributes;
using ShowCase.Services.Database;
using ShowCase.Services.PlantValue;

namespace ShowCase.Backend.Endpoints.PlantValue;

public class HydroComputerSocket : PlantValueSocket
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private int _plantId;
    public HydroComputerSocket(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override async Task OnConnect()
    {
        _plantId = int.Parse(RoomId.Split(":")[1]);

        await Broadcast("p:0");
    }

    [InvokeOn("p")]
    public async Task PumpStateChanged(PlantValueEvent @event)
        {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            await using (var dbContext = scope.ServiceProvider.GetRequiredService<KasDbContext>())
            {
                dbContext.PlantValues.Add(new PlantValueEntity()
                {
                    PlantId = _plantId,
                    PumpState = @event.PumpState,
                    RecordedAt = DateTime.UtcNow
                });
                await dbContext.SaveChangesAsync();
            }
        }

        await Broadcast("p:" + (@event.PumpState ? '1' : '0'));
    }
}