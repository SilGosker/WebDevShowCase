using Microsoft.EntityFrameworkCore;
using ShowCase.Services.Account;

namespace ShowCase.Services.Database;

public class KasDbContext : DbContext
{
    public KasDbContext(DbContextOptions<KasDbContext> options) : base(options)
    {
    }

    public DbSet<DbAccount> Accounts => Set<DbAccount>();
}