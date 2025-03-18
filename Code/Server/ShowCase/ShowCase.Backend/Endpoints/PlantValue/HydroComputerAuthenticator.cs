using System.Text;
using EasySockets.Authentication;
using EasySockets.Services;
using ShowCase.Services.Database;

namespace ShowCase.Backend.Endpoints.PlantValue;

public class HydroComputerAuthenticator : IEasySocketAsyncAuthenticator
{
    private readonly KasDbContext _kasDbContext;
    private readonly IEasySocketService _easySocketService;
    private readonly ILogger<PlantValueSocket> _logger;

    public HydroComputerAuthenticator(KasDbContext kasDbContext, IEasySocketService easySocketService, ILogger<PlantValueSocket> logger)
    {
        _kasDbContext = kasDbContext;
        _easySocketService = easySocketService;
        _logger = logger;
    }

    public async Task<EasySocketAuthenticationResult> AuthenticateAsync(EasySocketAuthenticationResult currentAuthenticationResult, HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out var authorizationValue) || string.IsNullOrEmpty(authorizationValue))
        {
            _logger.LogWarning("Request {Request} did not provide Authorization header", context.TraceIdentifier);
            return false;
        }

        // remove "Basic "
        var base64Bytes = Convert.FromBase64String(((string)authorizationValue!).Substring(6));
        var fromBase64 = Encoding.Default.GetString(base64Bytes);

        var split = fromBase64.Split(':');
        if (split.Length < 2)
        {
            _logger.LogWarning("Request {Request} provided invalid authorization header: {Base64Str}", context.TraceIdentifier, fromBase64);
            return false;
        }

        if (!int.TryParse(split[0], out int plantId))
        {
            _logger.LogWarning("Request {Request} provided invalid authorization header: {Base64Str}", context.TraceIdentifier, fromBase64);
            return false;
        }

        if (_easySocketService.Any("Plant:" + plantId, "Hydro"))
        {
            _logger.LogWarning("Request {Request} provided invalid authorization header: {Base64Str}", context.TraceIdentifier, fromBase64);
            return false;
        }

        var password = split[1];

        var plant = await _kasDbContext.Plants.FindAsync(plantId);
        if (plant is null)
        {
            _logger.LogWarning("Request {Request} provided invalid authorization header: {Base64Str}", context.TraceIdentifier, fromBase64);
            return false;
        }

        if (!BCrypt.Net.BCrypt.Verify(password, plant.Hash))
        {
            _logger.LogWarning("Request {Request} provided invalid authorization header: {Base64Str}", context.TraceIdentifier, fromBase64);
            return false;
        }

        return new EasySocketAuthenticationResult(true, "Plant:" + plant.Id, "Hydro");
    }
}