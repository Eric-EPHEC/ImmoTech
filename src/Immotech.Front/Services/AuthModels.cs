using System.Collections.Generic;

namespace Immotech.Front.Services;

// Simple DTO used by CookieAuthenticationStateProvider to capture API responses.
public class FormResult
{
    public bool Succeeded { get; set; }
    public string[] ErrorList { get; set; } = [];
    public string? Token { get; set; }
}

internal class UserInfo
{
    public string Email { get; set; } = string.Empty;
}
internal class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

internal class LoginResponse
{
    public string TokenType { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
}