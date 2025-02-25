using FastEndpoints;
using ShowCase.Services.Account;

namespace ShowCase.Backend.Endpoints.Account.Register;

public class RegisterEndpoint : Endpoint<RegisterRequest>
{
    private readonly IAccountService _accountService;

    public RegisterEndpoint(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/account/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        var account = await _accountService.CreateAccountAsync(req.Email, req.Password, ct);
        if (account == null)
        {
            await SendAsync(new
            {
                message = "Email already in use"
            }, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendOkAsync(ct);
    }
}