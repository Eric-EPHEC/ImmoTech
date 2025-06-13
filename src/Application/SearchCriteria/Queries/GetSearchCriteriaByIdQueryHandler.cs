using Application.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.SearchCriteria;

public class GetSearchCriteriaByIdQueryHandler : IRequestHandler<GetSearchCriteriaByIdQuery, Domain.Entities.SearchCriteria>
{
    private readonly IImmotechDbContext _context;

    public GetSearchCriteriaByIdQueryHandler(IImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.SearchCriteria> Handle(GetSearchCriteriaByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.SearchCriterias
            .AsNoTracking()
            .FirstOrDefaultAsync(sc => sc.Id == request.Id, cancellationToken);
    }
} 