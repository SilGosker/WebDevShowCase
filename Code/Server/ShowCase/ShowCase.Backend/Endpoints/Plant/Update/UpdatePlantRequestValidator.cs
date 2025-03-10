using FastEndpoints;

namespace ShowCase.Backend.Endpoints.Plant.Update;

public class UpdatePlantRequestValidator : Validator<UpdatePlantRequest>
{
    public UpdatePlantRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(250);
        RuleFor(x => x.Duration)
            .GreaterThan(0)
            .LessThanOrEqualTo(300);
    }
}