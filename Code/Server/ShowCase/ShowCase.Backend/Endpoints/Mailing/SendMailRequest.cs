using System.ComponentModel.DataAnnotations;

namespace ShowCase.Backend.Endpoints.Mailing;

public class SendMailRequest
{
    [Required(ErrorMessage = "Het emailadres is verplicht")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Het emailadres is ongeldig")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Het onderwerp is verplicht")]
    [MaxLength(200, ErrorMessage = "Het onderwerp mag niet langer zijn dan 200 karakters")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Het bericht is verplicht")] [MaxLength(600, ErrorMessage = "Het bericht mag niet langer zijn dan 600 karakters")]
    public string Body { get; set; } = string.Empty;
    [DataType(DataType.PhoneNumber, ErrorMessage = "het telefoonnumme is ongeldig")]
    public string? PhoneNumber { get; set; }
    public string? Name { get; set; }
}