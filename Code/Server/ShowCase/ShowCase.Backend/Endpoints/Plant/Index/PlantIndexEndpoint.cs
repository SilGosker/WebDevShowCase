using FastEndpoints;
using ShowCase.Services.Account;
using ShowCase.Services.Plants;

namespace ShowCase.Backend.Endpoints.Plant.Index;

public class PlantIndexEndpoint : EndpointWithoutRequest<PlantResponse[]>
{
    private readonly IPlantService _plantService;

    public PlantIndexEndpoint(IPlantService plantService)
    {
        _plantService = plantService;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/plants");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var accountId = User.Id();
        var plants = await _plantService.GetPlantsAsync(accountId, ct);
        var response = plants.Select(plant => new PlantResponse()
        {
            Id = plant.Id,
            Name = plant.Name
        }).ToArray();
        await SendOkAsync(response, ct);
    }
}