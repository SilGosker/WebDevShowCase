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

    public async Task<string?> CreatePlantAsync(Plant plant, CancellationToken ct)
    {
        var currentPlantCount = await _dbContext.Plants.CountAsync(x => x.AccountId == plant.AccountId, ct);
        
        if (currentPlantCount >= 5)
        {
            return null;
        }

        // remove the $2$a$ prefix from the password
        string password = BCrypt.Net.BCrypt.GenerateSalt()[5..];
        plant.Hash = password;
        _dbContext.Plants.Add(plant);
        await _dbContext.SaveChangesAsync(ct);
        return password;
    }

    public async Task<string?> UpdatePlantAsync(Plant plant, bool regeneratePassword, CancellationToken ct)
    {
        var dbPlant = await _dbContext.Plants.FirstOrDefaultAsync(x => x.AccountId == plant.AccountId && x.Id == plant.Id, ct);
        if (dbPlant == null)
        {
            return null;
        }

        dbPlant.Name = plant.Name;
        dbPlant.Duration = plant.Duration;

        string? password = null;
        if (regeneratePassword)
        {
            // remove the $2$a$ prefix from the password
            password = BCrypt.Net.BCrypt.GenerateSalt()[5..];
            dbPlant.Hash = password;
        }
        await _dbContext.SaveChangesAsync(ct);

        return password;
    }
}