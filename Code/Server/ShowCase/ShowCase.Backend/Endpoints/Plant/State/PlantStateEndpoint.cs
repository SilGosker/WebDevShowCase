using FastEndpoints;
using ShowCase.Services.Account;
using ShowCase.Services.Plants;

namespace ShowCase.Backend.Endpoints.Plant.State;

public class PlantStateEndpoint : EndpointWithoutRequest<PlantStateResponse>
{
    private readonly IPlantService _plantService;

    public PlantStateEndpoint(IPlantService plantService)
    {
        _plantService = plantService;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/plants/state/{Id:int}");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("Id");
        int accountId = User.Id();
        string? state = await _plantService.IsConnectedAsync(accountId, id);
        if (state == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var response = new PlantStateResponse()
        {
            State = state
        };

        await SendOkAsync(response, ct);
    }
}