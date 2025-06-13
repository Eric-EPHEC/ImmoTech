using Infrastructure.Persistences;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Notification;

public class GetNotificationByIdQuery : IRequest<GetNotificationByIdResponse>
{
    public int Id { get; set; }
}

public class GetNotificationByIdResponse
{
    public int Id { get; set; }
    public string? Message { get; set; }
    public DateTimeOffset SentAt { get; set; }
    public bool IsRead { get; set; }
    public Guid UserId { get; set; }
}

public class GetNotificationByIdQueryHandler : IRequestHandler<GetNotificationByIdQuery, GetNotificationByIdResponse>
{
    private readonly ImmotechDbContext _context;

    public GetNotificationByIdQueryHandler(ImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<GetNotificationByIdResponse> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
    {
        var notif = await _context.Notifications.AsNoTracking()
            .Where(n => n.Id == request.Id)
            .Select(n => new GetNotificationByIdResponse
            {
                Id = n.Id,
                Message = n.Message,
                SentAt = n.SentAt,
                IsRead = n.IsRead,
                UserId = n.UserId
            }).FirstOrDefaultAsync(cancellationToken);
        if (notif is null) throw new KeyNotFoundException($"Notification with ID {request.Id} not found.");
        return notif;
    }
} 