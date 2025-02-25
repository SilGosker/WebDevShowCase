namespace ShowCase.Backend.Endpoints.Account.Login;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}