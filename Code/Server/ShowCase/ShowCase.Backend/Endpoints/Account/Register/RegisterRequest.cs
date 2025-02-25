using System.ComponentModel.DataAnnotations;

namespace ShowCase.Backend.Endpoints.Account.Register;

public class RegisterRequest
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Range(8, 64)]
    public string Password { get; set; } = string.Empty;
}