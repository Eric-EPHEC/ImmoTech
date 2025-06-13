using System.Security.Claims;
using Application.Common;
using Microsoft.AspNetCore.Http;

namespace Immotech.Api.Common;

/// <summary>
/// This implementation of ICurrentUser is used to get the current user from the HttpContext.
/// HttpContext is only available in a web project.
/// </summary>
public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public Guid UserId
    {
        get
        {
            var identifier = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(identifier, out var id) ? id : throw new InvalidOperationException("User ID not found");
        }
    }

    public string? UserName => User?.Identity?.Name;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public bool IsInRole(string role) => User?.IsInRole(role) ?? false;
}
