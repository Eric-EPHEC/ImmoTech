using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<Guid>
{
    public ICollection<Property> Properties { get; set; } = [];
    public ICollection<SearchCriteria> SearchCriterias { get; set; } = [];
    public ICollection<Notification> Notifications { get; set; } = [];
}