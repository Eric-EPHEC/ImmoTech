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
    public Dictionary<string,string> Claims { get; set; } = new();
}

internal class RoleClaim
{
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string ValueType { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string OriginalIssuer { get; set; } = string.Empty;
} 