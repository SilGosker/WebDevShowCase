using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EasySockets.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShowCase.Backend.Configuration;
using ShowCase.Services.Account;
using ShowCase.Services.Database;

namespace ShowCase.Backend.Endpoints.PlantValue;

public class PlantWatcherAuthenticator : IEasySocketAsyncAuthenticator
{
    private readonly KasDbContext _dbContext;
    private readonly JwtOptions _jwtOptions;
    public PlantWatcherAuthenticator(KasDbContext dbContext, IOptions<JwtOptions> jwtOptions)
    {
        _dbContext = dbContext;
        _jwtOptions = jwtOptions.Value;
    }

    private ClaimsPrincipal? GetPrincipal(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtOptions.Audience,
            ValidateLifetime = true,
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            return principal;
        }
        catch (SecurityTokenException)
        {
            return null;
        }
    }

    public async Task<EasySocketAuthenticationResult> AuthenticateAsync(EasySocketAuthenticationResult currentAuthenticationResult, HttpContext context)
    {
        if (!context.Request.Query.TryGetValue("auth", out var authorizationHeader) | string.IsNullOrEmpty(authorizationHeader))
        {
            return false;
        }

        var principal = GetPrincipal(authorizationHeader!);
        if (principal is null)
        {
            return false;
        }

        if (principal.Identity is null or { IsAuthenticated: false })
        {
            return false;
        }

        if (!context.Request.Query.TryGetValue("plantId", out var plantIdStr) || !int.TryParse(plantIdStr, out int plantId))
        {
            return false;
        }
        var accountId = principal.Id();

        if (!await _dbContext.Plants.AnyAsync(p => p.Id == plantId && p.AccountId == accountId))
        {
            return false;
        }

        return new EasySocketAuthenticationResult(true, "Plant:" + plantId);
    }
}