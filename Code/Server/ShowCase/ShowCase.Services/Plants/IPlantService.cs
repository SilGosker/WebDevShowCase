namespace ShowCase.Services.Plants;

public interface IPlantService
{
    public Task<IEnumerable<Plant>> GetPlantsAsync(int accountId, CancellationToken ct);
    Task<string?> IsConnectedAsync(int accountId, int id);
    Task<Plant?> GetPlantAsync(int accountId, int id, CancellationToken ct);
    Task<string?> CreatePlantAsync(Plant plant, CancellationToken ct);
}