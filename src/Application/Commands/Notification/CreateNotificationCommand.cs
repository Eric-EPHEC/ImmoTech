using Infrastructure.Persistences;
using MediatR;

namespace Application.Commands.Notification;

public class CreateNotificationCommand : IRequest<CreateNotificationResponse>
{
    public required string Message { get; set; }
    public Guid UserId { get; set; }
}

public class CreateNotificationResponse
{
    public int Id { get; set; }
}

public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, CreateNotificationResponse>
{
    private readonly ImmotechDbContext _context;

    public CreateNotificationCommandHandler(ImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<CreateNotificationResponse> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = new Domain.Entities.Notification
        {
            Message = request.Message,
            SentAt = DateTimeOffset.UtcNow,
            IsRead = false,
            UserId = request.UserId
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateNotificationResponse { Id = notification.Id };
    }
} 