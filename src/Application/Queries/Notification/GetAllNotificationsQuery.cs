using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Notification;

public class GetAllNotificationsQuery : IRequest<GetAllNotificationsResponse>
{
    public Guid? UserId { get; set; }
    public bool? IsRead { get; set; }
}

public class GetAllNotificationsResponse
{
    public List<GetAllNotificationsResponseItem> Notifications { get; set; } = [];
    public int TotalCount { get; set; }
}

public class GetAllNotificationsResponseItem
{
    public int Id { get; set; }
    public string? Message { get; set; }
    public DateTimeOffset SentAt { get; set; }
    public bool IsRead { get; set; }
    public Guid UserId { get; set; }
}

public class GetAllNotificationsQueryHandler : IRequestHandler<GetAllNotificationsQuery, GetAllNotificationsResponse>
{
    private readonly IImmotechDbContext _context;

    public GetAllNotificationsQueryHandler(IImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<GetAllNotificationsResponse> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
        var notif = _context.Notifications.AsNoTracking();
        if (request.UserId.HasValue) notif = notif.Where(n => n.UserId == request.UserId.Value);
        if (request.IsRead.HasValue) notif = notif.Where(n => n.IsRead == request.IsRead.Value);

        var list = await notif.Select(n => new GetAllNotificationsResponseItem
        {
            Id = n.Id,
            Message = n.Message,
            SentAt = n.SentAt,
            IsRead = n.IsRead,
            UserId = n.UserId
        }).ToListAsync(cancellationToken);
        return new GetAllNotificationsResponse { Notifications = list, TotalCount = list.Count };
    }
} 