using Infrastructure.Persistences;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.SearchCriteria;

public class UpdateSearchCriteriaCommand : IRequest<UpdateSearchCriteriaResponse>
{
    public int Id { get; set; }
    public string? Keywords { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? Location { get; set; }
}

public class UpdateSearchCriteriaResponse
{
    public int Id { get; set; }
}

public class UpdateSearchCriteriaCommandHandler : IRequestHandler<UpdateSearchCriteriaCommand, UpdateSearchCriteriaResponse>
{
    private readonly ImmotechDbContext _context;

    public UpdateSearchCriteriaCommandHandler(ImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateSearchCriteriaResponse> Handle(UpdateSearchCriteriaCommand request, CancellationToken cancellationToken)
    {
        var crit = await _context.SearchCriterias.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (crit is null)
        {
            throw new KeyNotFoundException($"SearchCriteria with ID {request.Id} not found.");
        }
        if (request.Keywords is not null) crit.Keywords = request.Keywords;
        if (request.MinPrice.HasValue) crit.MinPrice = request.MinPrice.Value;
        if (request.MaxPrice.HasValue) crit.MaxPrice = request.MaxPrice.Value;
        if (request.Location is not null) crit.Location = request.Location;
        await _context.SaveChangesAsync(cancellationToken);
        return new UpdateSearchCriteriaResponse { Id = crit.Id };
    }
} 