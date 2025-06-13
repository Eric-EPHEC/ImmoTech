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
}

public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, DeleteNotificationResponse>
{
    private readonly IImmotechDbContext _context;

    public DeleteNotificationCommandHandler(IImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<DeleteNotificationResponse> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        await _context.Notifications.Where(n => n.Id == request.Id).ExecuteDeleteAsync(cancellationToken);

        return new DeleteNotificationResponse { Id = request.Id};
    }
} 