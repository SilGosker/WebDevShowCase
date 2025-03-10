using System.Data;
using System.Threading;
using FastEndpoints;
using ShowCase.Services.Account;
using ShowCase.Services.Plants;

namespace ShowCase.Backend.Endpoints.Plant.Update;

public class UpdatePlantEndpoint : Endpoint<UpdatePlantRequest, UpdatePlantResponse>
{
    private readonly IPlantService _plantService;

    public UpdatePlantEndpoint(IPlantService plantService)
    {
        _plantService = plantService;
    }

    public override void Configure()
    {
        Verbs(Http.PUT);
        Routes("/plants/update/{id:int}");
    }

    public override async Task HandleAsync(UpdatePlantRequest req, CancellationToken ct)
    {
        var id = Route<int>("id");
        var plant = new Services.Plants.Plant()
        {
            AccountId = User.Id(),
            Duration = req.Duration,
            Name = req.Name,
            Id = id
        };

        var password = await _plantService.UpdatePlantAsync(plant, req.RegeneratePassword, ct);

        await SendOkAsync(new UpdatePlantResponse
        {
            Id = plant.Id,
            Duration = plant.Duration,
            Name = plant.Name,
            Password = password
        }, ct);
    }
}