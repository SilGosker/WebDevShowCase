using FastEndpoints;
using ShowCase.Services.Configuration;
using ShowCase.Services.Mailing;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddFastEndpoints();
builder.Services.AddCors(options =>
{
    options.AddPolicy("SvelteApp", policy =>
    {
        var corsOrigins = builder.Configuration.GetSection("Cors:AllowAllOrigins").Get<string[]>()!;
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true)
            .WithOrigins(corsOrigins);
    });
});
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection(nameof(SmtpOptions)));

var app = builder.Build();

app.UseCors("SvelteApp");
app.UseHsts();
app.UseHttpsRedirection();

app.MapFastEndpoints();

app.Run();
