namespace Application.Common;

/// <summary>
/// Abstraction over the current authenticated user.  Implemented at the Infrastructure layer and injected
/// wherever access to the callers identity is required (e.g. auditing, authorization).
/// </summary>
public interface ICurrentUser
{
    /// <summary>
    /// Gets the unique identifier of the current user or <c>null</c> when the request is unauthenticated.
    /// </summary>
    Guid UserId { get; }

    /// <summary>
    /// Gets the user name / login of the current user if available.
    /// </summary>
    string? UserName { get; }

    /// <summary>
    /// Indicates whether the current request is authenticated.
    /// </summary>
    bool IsInRole(string role);
}
