using FastEndpoints;
using ShowCase.Services.Account;
using ShowCase.Services.Plants;

namespace ShowCase.Backend.Endpoints.Plant.Details;

public class PlantDetailsEndpoint : EndpointWithoutRequest<PlantDetailsResponse>
{
    private readonly IPlantService _plantService;

    public PlantDetailsEndpoint(IPlantService plantService)
    {
        _plantService = plantService;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/plants/{id:int}");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var accountId = User.Id();

        var plant = await _plantService.GetPlantAsync(accountId, id, ct);
        if (plant == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(new PlantDetailsResponse()
        {
            Name = plant.Name,
            Id = plant.Id,
            Duration = plant.Duration
        }, cancellation: ct);
        return;
    }
}