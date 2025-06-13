using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.SearchCriteria;

public class GetAllSearchCriteriasQuery : IRequest<GetAllSearchCriteriasResponse> { }

public class GetAllSearchCriteriasResponse
{
    public List<GetAllSearchCriteriasResponseItem> Criterias { get; set; } = [];
    public int TotalCount { get; set; }
}

public class GetAllSearchCriteriasResponseItem
{
    public int Id { get; set; }
    public string? Keywords { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public string? Location { get; set; }
}

public class GetAllSearchCriteriasQueryHandler : IRequestHandler<GetAllSearchCriteriasQuery, GetAllSearchCriteriasResponse>
{
    private readonly IImmotechDbContext _context;

    public GetAllSearchCriteriasQueryHandler(IImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<GetAllSearchCriteriasResponse> Handle(GetAllSearchCriteriasQuery request, CancellationToken cancellationToken)
    {
        var list = await _context.SearchCriterias.AsNoTracking().Select(c => new GetAllSearchCriteriasResponseItem
        {
            Id = c.Id,
            Keywords = c.Keywords,
            MinPrice = c.MinPrice,
            MaxPrice = c.MaxPrice,
            Location = c.Location
        }).ToListAsync(cancellationToken);
        return new GetAllSearchCriteriasResponse { Criterias = list, TotalCount = list.Count };
    }
} 