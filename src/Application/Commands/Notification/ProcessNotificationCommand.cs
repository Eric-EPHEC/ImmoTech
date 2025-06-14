using Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Notification;

/// <summary>
/// Process a single notification from the outbox: send the e-mail and mark it as sent.
/// </summary>
public class ProcessNotificationCommand : IRequest<ProcessNotificationResponse>
{
    public int Id { get; set; }
}

public class ProcessNotificationResponse
{
    public int Id { get; set; }
    public DateTimeOffset SentAt { get; set; }
}

public class ProcessNotificationCommandHandler : IRequestHandler<ProcessNotificationCommand, ProcessNotificationResponse>
{
    private readonly IImmotechDbContext _context;
    private readonly IEmailSender _emailSender;

    public ProcessNotificationCommandHandler(IImmotechDbContext context, IEmailSender emailSender)
    {
        _context = context;
        _emailSender = emailSender;
    }

    public async Task<ProcessNotificationResponse> Handle(ProcessNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);

        if (notification is null)
        {
            throw new KeyNotFoundException($"Notification {request.Id} not found.");
        }

        if (!string.IsNullOrWhiteSpace(notification.RecipientEmail))
        {
            await _emailSender.SendEmailAsync(notification.RecipientEmail, "Notification", notification.Message, cancellationToken);
        }

        notification.SentAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        return new ProcessNotificationResponse { Id = notification.Id, SentAt = notification.SentAt!.Value };
    }
}
