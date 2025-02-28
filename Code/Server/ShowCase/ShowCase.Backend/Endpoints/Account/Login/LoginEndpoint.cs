using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FastEndpoints;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShowCase.Backend.Configuration;
using ShowCase.Services.Account;

namespace ShowCase.Backend.Endpoints.Account.Login;

public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse?>
{
    private readonly IAccountService _accountService;
    private readonly JwtOptions _options;
    public LoginEndpoint(IAccountService accountService, IOptions<JwtOptions> options)
    {
        _accountService = accountService;
        _options = options.Value;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/account/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var account = await _accountService.LoginAsync(req.Email, req.Password, ct);
        if (account == null)
        {
            await SendAsync(null, StatusCodes.Status400BadRequest, ct);
            return;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_options.SecretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, account.Email),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Role.ToString()),
                new Claim("Id", account.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        await SendAsync(new LoginResponse()
        {
            Role = account.Role.ToString(),
            Token = tokenHandler.WriteToken(token)
        }, cancellation: ct);
    }
}