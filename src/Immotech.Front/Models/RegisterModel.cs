using System.ComponentModel.DataAnnotations;

namespace Immotech.Front.Models;

public class RegisterModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required, Compare("Password", ErrorMessage = "Passwords do not match")] 
    public string ConfirmPassword { get; set; } = string.Empty;
} 