namespace ShowCase.Services.Mailing;

public interface IEmailService
{
    public Task SendEmailAsync(string email, string subject, string body, CancellationToken ct = default);
}