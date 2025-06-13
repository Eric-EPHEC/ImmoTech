using Infrastructure.Persistences;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Notification;

public class DeleteNotificationCommand : IRequest<DeleteNotificationResponse>
{
    public int Id { get; set; }
}

public class DeleteNotificationResponse
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
}

public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, DeleteNotificationResponse>
{
    private readonly ImmotechDbContext _context;

    public DeleteNotificationCommandHandler(ImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<DeleteNotificationResponse> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        var notif = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);
        if (notif is null)
        {
            throw new KeyNotFoundException($"Notification with ID {request.Id} not found.");
        }

        _context.Notifications.Remove(notif);
        await _context.SaveChangesAsync(cancellationToken);
        return new DeleteNotificationResponse { Id = request.Id, IsDeleted = true };
    }
} 