using MediatR;

namespace Application.Commands.Notification;

public class CreateNotificationCommand : IRequest<CreateNotificationResponse>
{
    public required string Message { get; set; }
    public required string SenderEmail { get; set; }
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
        // validation
        var xor = request.RecipientId.HasValue ^ request.AgencyId.HasValue;
        if (!xor)
        {
            throw new ArgumentException("Specify RecipientId OR AgencyId (not both / none).");
        }
        if (string.IsNullOrWhiteSpace(request.SenderEmail))
        {
            throw new ArgumentException("SenderEmail is required.");
        }

        var notification = new Domain.Entities.Notification
        {
            Message = request.Message,
            SentAt = DateTimeOffset.UtcNow,
            IsRead = false,
            SenderEmail = request.SenderEmail,
            RecipientId = request.RecipientId,
            AgencyId = request.AgencyId
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateNotificationResponse { Id = notification.Id };
    }
} 