using System.ComponentModel.DataAnnotations;

namespace ShowCase.Services.Database;

public class DbEntity
{
    [Key]
    public int Id { get; set; }

    public bool Deleted { get; set; }
}