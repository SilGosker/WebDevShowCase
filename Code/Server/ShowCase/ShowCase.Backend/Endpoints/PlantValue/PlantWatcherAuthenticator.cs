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
    private readonly ILogger<PlantWatcherAuthenticator> _logger;
    public PlantWatcherAuthenticator(KasDbContext dbContext, IOptions<JwtOptions> jwtOptions, ILogger<PlantWatcherAuthenticator> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
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
            _logger.LogWarning("Request {Request} provided invalid authorization parameter: {AuthParameter}", context.TraceIdentifier, authorizationHeader);
            return false;
        }

        if (principal.Identity is null or { IsAuthenticated: false })
        {
            _logger.LogWarning("Request {Request} provided invalid authorization parameter: {AuthParameter}", context.TraceIdentifier, authorizationHeader);
            return false;
        }

        if (!context.Request.Query.TryGetValue("plantId", out var plantIdStr) || !int.TryParse(plantIdStr, out int plantId))
        {
            _logger.LogWarning("Request {Request} provided valid authorization but invalid plantId: {AuthParameter}", context.TraceIdentifier, plantIdStr);
            return false;
        }
        var accountId = principal.Id();

        if (!await _dbContext.Plants.AnyAsync(p => p.Id == plantId && p.AccountId == accountId))
        {
            _logger.LogWarning("Request {Request} provided valid authorization but invalid plantId: {AuthParameter}", context.TraceIdentifier, plantIdStr);
            return false;
        }

        return new EasySocketAuthenticationResult(true, "Plant:" + plantId);
    }
}