using FastEndpoints;

namespace ShowCase.Backend.Endpoints.Account.Login;

public class RegisterRequestValidator : Validator<LoginRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(64);
    }
}