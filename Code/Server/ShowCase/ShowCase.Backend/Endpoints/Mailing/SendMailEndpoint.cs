using System.Text;
using FastEndpoints;
using ShowCase.Services.Mailing;

namespace ShowCase.Backend.Endpoints.Mailing;

public class SendMailEndpoint : Endpoint<SendMailRequest>
{
    private readonly IEmailService _emailService;

    public SendMailEndpoint(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public override void Configure()
    {
        AllowAnonymous();
        Verbs(Http.POST);
        Routes("/mailing/send");
    }

    public override async Task HandleAsync(SendMailRequest req, CancellationToken ct)
    {
        if (ValidationFailed)
        {
            await SendAsync(ValidationFailures.Select(e => new { field = e.PropertyName, error = e.ErrorMessage }),
                StatusCodes.Status400BadRequest, ct);
            return;
        }

        StringBuilder sb = new(req.Body);
        sb
            .AppendLine()
            .AppendLine()
            .AppendLine("Contactgegevens: ")
            .Append("Naam: ")
            .AppendLine(req.Name)
            .Append("Email: ")
            .AppendLine(req.Email)
            .Append("Telefoonnummer: ")
            .AppendLine(req.PhoneNumber);

        await _emailService.SendEmailAsync(req.Email, req.Subject, sb.ToString(), ct);
        await SendOkAsync(ct);
    }
}