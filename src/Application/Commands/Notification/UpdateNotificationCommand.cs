using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Notification;

public class UpdateNotificationCommand : IRequest<UpdateNotificationResponse>
{
    public int Id { get; set; }
    public string? Message { get; set; }
    public bool? IsRead { get; set; }
}

public class UpdateNotificationResponse
{
    public int Id { get; set; }
}

public class UpdateNotificationCommandHandler : IRequestHandler<UpdateNotificationCommand, UpdateNotificationResponse>
{
    private readonly IImmotechDbContext _context;

    public UpdateNotificationCommandHandler(IImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateNotificationResponse> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notif = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);
        if (notif is null)
        {
            throw new KeyNotFoundException($"Notification with ID {request.Id} not found.");
        }
        if (request.Message is not null) notif.Message = request.Message;
        if (request.IsRead.HasValue) notif.IsRead = request.IsRead.Value;

        await _context.SaveChangesAsync(cancellationToken);
        return new UpdateNotificationResponse { Id = notif.Id };
    }
} 