using Microsoft.EntityFrameworkCore;
using ShowCase.Services.Account;
using ShowCase.Services.Plants;
using ShowCase.Services.PlantValue;

namespace ShowCase.Services.Database;

public class KasDbContext : DbContext
{
    public KasDbContext(DbContextOptions<KasDbContext> options) : base(options)
    {
    }

    public DbSet<DbAccount> Accounts => Set<DbAccount>();
    public DbSet<Plant> Plants => Set<Plant>();
    public DbSet<PlantValueEntity> PlantValues => Set<PlantValueEntity>();
}