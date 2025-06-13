using Infrastructure.Persistences;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.ModerationLog;

public class DeleteModerationLogCommand : IRequest<DeleteModerationLogResponse>
{
    public int Id { get; set; }
}

public class DeleteModerationLogResponse
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
}

public class DeleteModerationLogCommandHandler : IRequestHandler<DeleteModerationLogCommand, DeleteModerationLogResponse>
{
    private readonly ImmotechDbContext _context;

    public DeleteModerationLogCommandHandler(ImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<DeleteModerationLogResponse> Handle(DeleteModerationLogCommand request, CancellationToken cancellationToken)
    {
        var log = await _context.ModerationLogs.FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);
        if (log is null)
        {
            throw new KeyNotFoundException($"ModerationLog with ID {request.Id} not found.");
        }
        _context.ModerationLogs.Remove(log);
        await _context.SaveChangesAsync(cancellationToken);
        return new DeleteModerationLogResponse { Id = request.Id, IsDeleted = true };
    }
} 