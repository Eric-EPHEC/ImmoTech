using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.ModerationLog;

public class GetAllModerationLogsQuery : IRequest<GetAllModerationLogsResponse>
{
    public Guid? PropertyId { get; set; }
}

public class GetAllModerationLogsResponse
{
    public List<GetAllModerationLogsResponseItem> Logs { get; set; } = [];
    public int TotalCount { get; set; }
}

public class GetAllModerationLogsResponseItem
{
    public int Id { get; set; }
    public Guid PropertyId { get; set; }
    public Guid ModeratorId { get; set; }
    public string? Action { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}

public class GetAllModerationLogsQueryHandler : IRequestHandler<GetAllModerationLogsQuery, GetAllModerationLogsResponse>
{
    private readonly IImmotechDbContext _context;

    public GetAllModerationLogsQueryHandler(IImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<GetAllModerationLogsResponse> Handle(GetAllModerationLogsQuery request, CancellationToken cancellationToken)
    {
        var logs = _context.ModerationLogs.AsNoTracking();
        if (request.PropertyId.HasValue) logs = logs.Where(l => l.PropertyId == request.PropertyId.Value);
        var list = await logs.Select(l => new GetAllModerationLogsResponseItem
        {
            Id = l.Id,
            PropertyId = l.PropertyId,
            ModeratorId = l.ModeratorId,
            Action = l.Action,
            Timestamp = l.Timestamp
        }).ToListAsync(cancellationToken);
        return new GetAllModerationLogsResponse { Logs = list, TotalCount = list.Count };
    }
} 