using Microsoft.EntityFrameworkCore;
using ShowCase.Services.Database;

namespace ShowCase.Services.Account;

public class AccountService : IAccountService
{
    private readonly KasDbContext _dbContext;

    public AccountService(KasDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DbAccount?> CreateAccountAsync(string email, string password, CancellationToken ct = default)
    {
        if (await _dbContext.Accounts.AnyAsync(a => a.Email == email, ct))
        {
            return null;
        }

        var account = new DbAccount
        {
            Email = email,
            Salt = BCrypt.Net.BCrypt.GenerateSalt(),
        };
        account.Hash = BCrypt.Net.BCrypt.HashPassword(password, account.Salt);

        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync(ct);
        return account;
    }

    public async Task<DbAccount?> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Email == email, ct);
        if (account == null)
        {
            return null;
        }

        if (!BCrypt.Net.BCrypt.Verify(password, account.Hash))
        {
            return null;
        }

        return account;
    }
}