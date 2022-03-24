using System.ComponentModel.DataAnnotations;

namespace PaymentWebClient.Models;

public class RegisterAccountForm
{
    [Required]
    public string Username { get; set; }

    [Required]
    [StringLength(12, ErrorMessage = "Password must be at least 8 characters long.", MinimumLength = 12)]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password))]
    public string Password2 { get; set; }
}