using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.SearchCriteria;

public class GetSearchCriteriaByIdQuery : IRequest<GetSearchCriteriaByIdResponse>
{
    public int Id { get; set; }
}

public class GetSearchCriteriaByIdResponse
{
    public int Id { get; set; }
    public string? Keywords { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public string? Location { get; set; }
}

public class GetSearchCriteriaByIdQueryHandler : IRequestHandler<GetSearchCriteriaByIdQuery, GetSearchCriteriaByIdResponse>
{
    private readonly IImmotechDbContext _context;

    public GetSearchCriteriaByIdQueryHandler(IImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<GetSearchCriteriaByIdResponse> Handle(GetSearchCriteriaByIdQuery request, CancellationToken cancellationToken)
    {
        var crit = await _context.SearchCriterias.AsNoTracking().Where(c => c.Id == request.Id)
            .Select(c => new GetSearchCriteriaByIdResponse
            {
                Id = c.Id,
                Keywords = c.Keywords,
                MinPrice = c.MinPrice,
                MaxPrice = c.MaxPrice,
                Location = c.Location
            }).FirstOrDefaultAsync(cancellationToken);
        if (crit is null) throw new KeyNotFoundException($"SearchCriteria with ID {request.Id} not found.");
        return crit;
    }
} 