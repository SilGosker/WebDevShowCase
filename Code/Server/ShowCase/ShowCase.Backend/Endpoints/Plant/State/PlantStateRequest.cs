using Microsoft.AspNetCore.Mvc;

namespace ShowCase.Backend.Endpoints.Plant.State;

public class PlantStateRequest
{
    [FromRoute]
    public int Id { get; set; }
}