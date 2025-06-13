
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.SearchCriteria;

public class DeleteSearchCriteriaCommand : IRequest<DeleteSearchCriteriaResponse>
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
}

public class DeleteSearchCriteriaResponse
{
    public int Id { get; set; }

   
}

public class DeleteSearchCriteriaCommandHandler : IRequestHandler<DeleteSearchCriteriaCommand, DeleteSearchCriteriaResponse>
{
    private readonly IImmotechDbContext _context;

    public DeleteSearchCriteriaCommandHandler(IImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<DeleteSearchCriteriaResponse> Handle(DeleteSearchCriteriaCommand request, CancellationToken cancellationToken)
    {
        await _context.SearchCriterias.Where(c => c.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
        return new DeleteSearchCriteriaResponse { Id = request.Id};
    }
} 