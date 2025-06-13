using Infrastructure.Persistences;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.SearchCriteria;

public class DeleteSearchCriteriaCommand : IRequest<DeleteSearchCriteriaResponse>
{
    public int Id { get; set; }
}

public class DeleteSearchCriteriaResponse
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
}

public class DeleteSearchCriteriaCommandHandler : IRequestHandler<DeleteSearchCriteriaCommand, DeleteSearchCriteriaResponse>
{
    private readonly ImmotechDbContext _context;

    public DeleteSearchCriteriaCommandHandler(ImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<DeleteSearchCriteriaResponse> Handle(DeleteSearchCriteriaCommand request, CancellationToken cancellationToken)
    {
        var crit = await _context.SearchCriterias.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (crit is null)
        {
            throw new KeyNotFoundException($"SearchCriteria with ID {request.Id} not found.");
        }

        _context.SearchCriterias.Remove(crit);
        await _context.SaveChangesAsync(cancellationToken);
        return new DeleteSearchCriteriaResponse { Id = request.Id, IsDeleted = true };
    }
} 