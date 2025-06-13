using MediatR;

namespace Application.Commands.SearchCriteria;

public class CreateSearchCriteriaCommand : IRequest<CreateSearchCriteriaResponse>
{
    public string? Keywords { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? Location { get; set; }
    public Guid UserId { get; set; }
}

public class CreateSearchCriteriaResponse
{
    public int Id { get; set; }
}

public class CreateSearchCriteriaCommandHandler : IRequestHandler<CreateSearchCriteriaCommand, CreateSearchCriteriaResponse>
{
    private readonly IImmotechDbContext _context;

    public CreateSearchCriteriaCommandHandler(IImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<CreateSearchCriteriaResponse> Handle(CreateSearchCriteriaCommand request, CancellationToken cancellationToken)
    {
        var criteria = new Domain.Entities.SearchCriteria
        {
            Keywords = request.Keywords ?? string.Empty,
            MinPrice = request.MinPrice ?? 0,
            MaxPrice = request.MaxPrice ?? 0,
            Location = request.Location ?? string.Empty
        };
        _context.SearchCriterias.Add(criteria);
        await _context.SaveChangesAsync(cancellationToken);
        return new CreateSearchCriteriaResponse { Id = criteria.Id };
    }
} 