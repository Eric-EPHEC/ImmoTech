using MediatR;

namespace Application.Commands.Notification;

public class CreateNotificationCommand : IRequest<CreateNotificationResponse>
{
    public required string Message { get; set; }
    public Guid SenderId { get; set; }
    public Guid? RecipientId { get; set; }
    public int? AgencyId { get; set; }
}

public class CreateNotificationResponse
{
    public int Id { get; set; }
}

public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, CreateNotificationResponse>
{
    private readonly IImmotechDbContext _context;

    public CreateNotificationCommandHandler(IImmotechDbContext context)
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
            SenderId = request.SenderId,
            RecipientId = request.RecipientId,
            AgencyId = request.AgencyId
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateNotificationResponse { Id = notification.Id };
    }
} 