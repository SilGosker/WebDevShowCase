namespace ShowCase.Services.Account;

public interface IAccountService
{
    public Task<DbAccount?> CreateAccountAsync(string email, string password, CancellationToken ct);
    public Task<DbAccount?> LoginAsync(string email, string password, CancellationToken ct);
}