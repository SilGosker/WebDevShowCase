using System.ComponentModel.DataAnnotations;
using ShowCase.Services.Database;

namespace ShowCase.Services.Account;

public class DbAccount : DbEntity
{
    [MaxLength(255)]
    public string Email { get; set; } = null!;
    [MaxLength(255)]
    public string Salt { get; set; } = null!;
    [MaxLength(255)]
    public string Hash { get; set; } = null!;

    public Role Role { get; set; } = Role.PlantHolder;
}