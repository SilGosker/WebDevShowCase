﻿using FastEndpoints;

namespace ShowCase.Backend.Endpoints.Plant.Create;

public class CreatePlantRequestValidator : Validator<CreatePlantRequest>
{
    public CreatePlantRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(250);
        RuleFor(x => x.Duration)
            .GreaterThan(0)
            .LessThanOrEqualTo(300);
    }
}