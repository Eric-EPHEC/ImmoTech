using Infrastructure.Persistences;
using MediatR;

namespace Application.Commands.ModerationLog;

public class CreateModerationLogCommand : IRequest<CreateModerationLogResponse>
{
    public Guid PropertyId { get; set; }
    public Guid ModeratorId { get; set; }
    public required string Action { get; set; }
}

public class CreateModerationLogResponse
{
    public int Id { get; set; }
}

public class CreateModerationLogCommandHandler : IRequestHandler<CreateModerationLogCommand, CreateModerationLogResponse>
{
    private readonly ImmotechDbContext _context;

    public CreateModerationLogCommandHandler(ImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<CreateModerationLogResponse> Handle(CreateModerationLogCommand request, CancellationToken cancellationToken)
    {
        var log = new Domain.Entities.ModerationLog
        {
            PropertyId = request.PropertyId,
            ModeratorId = request.ModeratorId,
            Action = request.Action,
            Timestamp = DateTimeOffset.UtcNow
        };
        _context.ModerationLogs.Add(log);
        await _context.SaveChangesAsync(cancellationToken);
        return new CreateModerationLogResponse { Id = log.Id };
    }
} 