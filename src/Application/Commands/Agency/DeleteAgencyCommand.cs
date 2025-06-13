using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Agency;

public class DeleteAgencyCommand : IRequest<DeleteAgencyResponse>
{
    public int Id { get; set; }
}

public class DeleteAgencyResponse
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
}

public class DeleteAgencyCommandHandler : IRequestHandler<DeleteAgencyCommand, DeleteAgencyResponse>
{
    private readonly IImmotechDbContext _context;

    public DeleteAgencyCommandHandler(IImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<DeleteAgencyResponse> Handle(DeleteAgencyCommand request, CancellationToken cancellationToken)
    {
        var agency = await _context.Agencies.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (agency is null)
        {
            throw new KeyNotFoundException($"Agency with ID {request.Id} not found.");
        }

        _context.Agencies.Remove(agency);
        await _context.SaveChangesAsync(cancellationToken);

        return new DeleteAgencyResponse { Id = request.Id, IsDeleted = true };
    }
} 