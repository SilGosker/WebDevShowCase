using EasySockets.Services;
using Microsoft.EntityFrameworkCore;
using ShowCase.Backend.Extensions;
using ShowCase.Services.Database;
using Timer = System.Timers.Timer;
namespace ShowCase.Backend.BackgroundServices;

public class WateringBackgroundService : IHostedService
{
    private const int WateringHour = 19;
    private readonly IEasySocketService _easySocketService;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private Timer? _timer;
    public WateringBackgroundService(IEasySocketService easySocketService, IServiceScopeFactory serviceScopeFactory)
    {
        _easySocketService = easySocketService;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer();
        _timer.Interval = 1 * 60 * 60 * 1000;
        _timer.Elapsed += (_, _) =>
        {
            if (DateTime.Now.Hour == WateringHour)
            {
                WaterPlants().Forget();
            }
        };

        _timer!.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer!.Stop();
        return Task.CompletedTask;
    }

    private async Task WaterPlants()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<KasDbContext>();
            foreach (var plant in await dbContext.Plants.ToArrayAsync())
            {
                if (_easySocketService.Any("Plant:" + plant.Id, "Hydro"))
                {
                    await _easySocketService.SendToRoomAsync("Plant:" + plant.Id, "Water");
                }
            }
        }
    }
}