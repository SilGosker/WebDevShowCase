using System.ComponentModel.DataAnnotations.Schema;
using ShowCase.Services.Account;
using ShowCase.Services.Database;

namespace ShowCase.Services.Plants;

public class Plant : DbEntity
{
    public int AccountId { get; set; }

    [ForeignKey(nameof(AccountId))] public DbAccount? Account { get; set; }
    public string Name { get; set; } = string.Empty;

}