using System.ComponentModel.DataAnnotations;

namespace ShowCase.Backend.Endpoints.Account.Register;

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}