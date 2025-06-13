using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.ModerationLog;

public class GetModerationLogByIdQuery : IRequest<GetModerationLogByIdResponse>
{
    public int Id { get; set; }
}

public class GetModerationLogByIdResponse
{
    public int Id { get; set; }
    public Guid PropertyId { get; set; }
    public Guid ModeratorId { get; set; }
    public string? Action { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}

public class GetModerationLogByIdQueryHandler : IRequestHandler<GetModerationLogByIdQuery, GetModerationLogByIdResponse>
{
    private readonly IImmotechDbContext _context;

    public GetModerationLogByIdQueryHandler(IImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<GetModerationLogByIdResponse> Handle(GetModerationLogByIdQuery request, CancellationToken cancellationToken)
    {
        var log = await _context.ModerationLogs.AsNoTracking().Where(l => l.Id == request.Id)
            .Select(l => new GetModerationLogByIdResponse
            {
                Id = l.Id,
                PropertyId = l.PropertyId,
                ModeratorId = l.ModeratorId,
                Action = l.Action,
                Timestamp = l.Timestamp
            }).FirstOrDefaultAsync(cancellationToken);
        if (log is null) throw new KeyNotFoundException($"ModerationLog with ID {request.Id} not found.");
        return log;
    }
} 