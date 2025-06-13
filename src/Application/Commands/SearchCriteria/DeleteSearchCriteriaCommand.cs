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
        await _context.SearchCriterias.Where(c => c.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
        return new DeleteSearchCriteriaResponse { Id = request.Id};
    }
} 