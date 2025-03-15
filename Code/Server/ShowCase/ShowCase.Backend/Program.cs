global using FluentValidation;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ShowCase.Services.Account;
using ShowCase.Services.Configuration;
using ShowCase.Services.Mailing;
using System.Text;
using System.Text.Json;
using EasySockets.Builder;
using Microsoft.EntityFrameworkCore;
using ShowCase.Backend.Configuration;
using ShowCase.Services.Database;
using Microsoft.IdentityModel.Tokens;
using ShowCase.Backend.BackgroundServices;
using ShowCase.Backend.Endpoints.PlantValue;
using ShowCase.Services.Plants;

AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromSeconds(3));

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowAllOrigins").Get<string[]>()!;
var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(jwtOptions.SecretKey))
        };
    });

builder.Services.AddEasySocketServices();

builder.Services.AddDbContext<KasDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("PlantHolderPolicy", policy => policy.RequireRole("PlantHolder"));
});

builder.Services.AddFastEndpoints();
builder.Services.AddCors(options =>
{
    options.AddPolicy("SvelteApp", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true)
            .WithOrigins(allowedOrigins);
    });
});

builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddScoped<IPlantService, PlantService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection(nameof(SmtpOptions)));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddHostedService<WateringBackgroundService>();

var app = builder.Build();

app.UseHsts();

app.UseEasySockets()
    .AddEasySocket<HydroComputerSocket>("/hydro",
        options => { options.AddAsyncAuthenticator<HydroComputerAuthenticator>(); })
    .AddEasySocket<PlantWatcherSocket>("/watch", options =>
    {
        options.AddAsyncAuthenticator<PlantWatcherAuthenticator>();
    });


app.UseHttpsRedirection();
app.UseCors("SvelteApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapFastEndpoints(options =>
{
    options.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

app.Run();

public partial class Program;