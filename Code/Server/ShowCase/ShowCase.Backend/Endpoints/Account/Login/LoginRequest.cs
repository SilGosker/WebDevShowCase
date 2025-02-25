using System.ComponentModel.DataAnnotations;

namespace ShowCase.Backend.Endpoints.Account.Login;

public class LoginRequest
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Range(8, 64)]
    public string Password { get; set; } = string.Empty;
}