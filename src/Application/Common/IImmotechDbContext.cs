using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common;

/// <summary>
/// Abstraction of IImmotechDbContext to remove the dependency on Infrastructure.
/// </summary>
public interface IImmotechDbContext
{
    DbSet<Property> Properties { get; }
    DbSet<Agency> Agencies { get; }
    DbSet<ProfessionalUser> ProfessionalUsers { get; }
    DbSet<Photo> Photos { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<ModerationLog> ModerationLogs { get; }
    DbSet<SearchCriteria> SearchCriterias { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
