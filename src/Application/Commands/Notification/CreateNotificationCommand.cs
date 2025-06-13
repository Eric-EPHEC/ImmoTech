using MediatR;
using Microsoft.AspNetCore.Identity;
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
    private readonly IImmotechDbContext _context; // db context of my own DB
    private readonly UserManager<Domain.Entities.User> _userManager; // user manager of EntityFrameworkIdentity
    public CreateNotificationCommandHandler(IImmotechDbContext context, UserManager<Domain.Entities.User> userManager) // dependency injection
    {
        _context = context; // db context of my own DB
        _userManager = userManager; // user manager of EntityFrameworkIdentity
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

        string recipientEmail;

        if (request.RecipientId.HasValue )
        {
            recipientEmail = (await _userManager.FindByIdAsync(request.RecipientId.ToString()))!.NormalizedEmail!;
        }

        else
        {
            recipientEmail = (await _context.Agencies.FindAsync(request.AgencyId))?.ContactEmail!;
        }

        var notification = new Domain.Entities.Notification
        {
            Message = request.Message,
            SentAt = null,
            IsRead = false,
            SenderEmail = request.SenderEmail,
            RecipientId = request.RecipientId,
            AgencyId = request.AgencyId,
            RecipientEmail = recipientEmail
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateNotificationResponse { Id = notification.Id };
    }
} 