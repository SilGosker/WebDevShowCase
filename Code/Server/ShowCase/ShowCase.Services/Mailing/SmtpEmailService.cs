using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using ShowCase.Services.Configuration;

namespace ShowCase.Services.Mailing;

public class SmtpEmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;
    private readonly SmtpOptions _options;
    public SmtpEmailService(IOptions<SmtpOptions> options)
    {
        _options = options.Value;
        _smtpClient = new(_options.Host, _options.Port);
        _smtpClient.EnableSsl = _options.EnableSsl;
        _smtpClient.Credentials = new NetworkCredential(_options.Username, _options.Password);
    }

    public async Task SendEmailAsync(string email, string subject, string body, CancellationToken ct)
    {
        MailMessage mail = new(_options.From, email, subject, body);
        await _smtpClient.SendMailAsync(mail, ct);
    }
}