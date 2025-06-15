namespace Application.Common;

public interface ICurrentUser
{

    Guid UserId { get; }
    string? UserName { get; }

    int? AgencyId { get; }
}
