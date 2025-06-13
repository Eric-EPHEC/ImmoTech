namespace Application.Common;

public interface ICurrentUser
{

    Guid UserId { get; }
    string? UserName { get; }
    bool IsInRole(string role);

    int? AgencyId { get; }
}
