using EasySockets.Services;
using Microsoft.EntityFrameworkCore;
using ShowCase.Services.Database;

namespace ShowCase.Services.Plants;

public class PlantService : IPlantService
{
    private readonly KasDbContext _dbContext;
    private readonly IEasySocketService _easySocketService;

    public PlantService(KasDbContext dbContext, IEasySocketService easySocketService)
    {
        _dbContext = dbContext;
        _easySocketService = easySocketService;
    }

    public async Task<IEnumerable<Plant>> GetPlantsAsync(int accountId, CancellationToken ct)
    {
        return await _dbContext.Plants.Where(plant => plant.AccountId == accountId).ToListAsync(ct);
    }

    
    public async Task<string?> IsConnectedAsync(int accountId, int id)
    {
        if (!await _dbContext.Plants.AnyAsync(x => x.AccountId == accountId && x.Id == id))
        {
            return null;
        }

        return _easySocketService.Any($"Plant{id}", "Controller")
            ? "Connected"
            : "Disconnected";
    }

    public async Task<Plant?> GetPlantAsync(int accountId, int id, CancellationToken cancellationToken)
    {
        var plant = await _dbContext.Plants.FindAsync(keyValues: [id], cancellationToken);

        if (plant?.AccountId != accountId)
        {
            return null;
        }

        return plant;
    }
}