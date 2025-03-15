using EasySockets.Attributes;
using EasySockets.Services;
using ShowCase.Services.Database;
using ShowCase.Services.Plants;

namespace ShowCase.Backend.Endpoints.PlantValue;

public class PlantWatcherSocket : PlantValueSocket
{
    private readonly IEasySocketService _easySocketService;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private int _plantId;
    public PlantWatcherSocket(IEasySocketService easySocketService, IServiceScopeFactory serviceScopeFactory)
    {
        _easySocketService = easySocketService;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override Task OnConnect()
    {
        _plantId = int.Parse(RoomId.Substring(6));
        return Task.CompletedTask;
    }

    [InvokeOn("p")]
    public async Task GiveWater(PlantValueEvent @event)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            await using (var dbContext = scope.ServiceProvider.GetRequiredService<KasDbContext>())
            {
                var plant = await dbContext.Plants.FindAsync(_plantId);
                await _easySocketService.SendToClientAsync(RoomId, "Hydro", "p:" + plant!.Duration);
            }
        }
    }
}