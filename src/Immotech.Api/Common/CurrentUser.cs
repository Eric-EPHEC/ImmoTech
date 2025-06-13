using System.Security.Claims;
using Application.Common;
using Microsoft.AspNetCore.Http;

namespace Immotech.Api.Common;

/// <summary>
/// Runtime implementation of <see cref="ICurrentUser"/> that extracts information from the current <see cref="HttpContext"/>.
/// </summary>
public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public Guid? UserId
    {
        get
        {
            var identifier = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(identifier, out var id) ? id : null;
        }
    }

    public string? UserName => User?.Identity?.Name;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public bool IsInRole(string role) => User?.IsInRole(role) ?? false;
}
