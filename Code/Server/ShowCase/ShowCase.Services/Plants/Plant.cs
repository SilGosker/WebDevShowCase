using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShowCase.Services.Account;
using ShowCase.Services.Database;
using ShowCase.Services.PlantValue;

namespace ShowCase.Services.Plants;

public class Plant : DbEntity
{
    public int AccountId { get; set; }

    [ForeignKey(nameof(AccountId))] public DbAccount? Account { get; set; }
    [MaxLength(250)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(250)] public string Hash { get; set; } = string.Empty;
    public int Duration { get; set; }

    public virtual ICollection<PlantValueEntity> PlantValues { get; set; } = new List<PlantValueEntity>();
}