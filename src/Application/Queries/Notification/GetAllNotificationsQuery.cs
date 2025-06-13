using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Notification;

public class GetAllNotificationsQuery : IRequest<GetAllNotificationsResponse>
{
    public Guid? RecipientId { get; set; }
    public int? AgencyId { get; set; }
    public string? SenderEmail { get; set; }
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
    public string? SenderEmail { get; set; }
    public Guid? RecipientId { get; set; }
    public int? AgencyId { get; set; }
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

        if (request.RecipientId.HasValue)
            notif = notif.Where(n => n.RecipientId == request.RecipientId.Value);

        if (request.AgencyId.HasValue)
            notif = notif.Where(n => n.AgencyId == request.AgencyId.Value);

        if (!string.IsNullOrWhiteSpace(request.SenderEmail))
            notif = notif.Where(n => n.SenderEmail == request.SenderEmail);

        if (request.IsRead.HasValue)
            notif = notif.Where(n => n.IsRead == request.IsRead.Value);

        var list = await notif.Select(n => new GetAllNotificationsResponseItem
        {
            Id = n.Id,
            Message = n.Message,
            SentAt = n.SentAt,
            IsRead = n.IsRead,
            SenderEmail = n.SenderEmail,
            RecipientId = n.RecipientId,
            AgencyId = n.AgencyId
        }).ToListAsync(cancellationToken);

        return new GetAllNotificationsResponse { Notifications = list, TotalCount = list.Count };
    }
} 