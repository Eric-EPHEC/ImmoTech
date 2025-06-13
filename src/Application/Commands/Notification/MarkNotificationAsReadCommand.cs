using Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Notification;

public class MarkNotificationAsReadCommand : IRequest
{
    public int Id { get; set; }
} 


public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand>
{
    private readonly IImmotechDbContext _context;

    public MarkNotificationAsReadCommandHandler(IImmotechDbContext context)
    {
        _context = context;
    }

    public async Task Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);

        if (notification == null)
        {
            throw new Exception("Notification not found");
        }

        notification.IsRead = true;
        await _context.SaveChangesAsync(cancellationToken);

    }
} 