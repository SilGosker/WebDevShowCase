using FastEndpoints;
using ShowCase.Services.Account;
using ShowCase.Services.Plants;

namespace ShowCase.Backend.Endpoints.Plant.Create;

public class CreatePlantEndpoint : Endpoint<CreatePlantRequest, CreatePlantResponse>
{
    private readonly IPlantService _plantService;

    public CreatePlantEndpoint(IPlantService plantService)
    {
        _plantService = plantService;
    }

    public override void Configure()
    {
        Routes("/plants/create");
        Verbs(Http.POST);
    }

    public override async Task HandleAsync(CreatePlantRequest req, CancellationToken ct)
    {
       
        var accountId = User.Id();
        var plant = new Services.Plants.Plant
        {
            Name = req.Name,
            Duration = req.Duration,
            AccountId = accountId
        };
        var password = await _plantService.CreatePlantAsync(plant, ct);
        
        if (password == null)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var response = new CreatePlantResponse
        {
            Password = password,
            Duration = plant.Duration,
            Name = plant.Name,
            Id = plant.Id
        };

        await SendAsync(response, cancellation: ct);
    }
}