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
        crit.Keywords = request.Keywords ?? string.Empty;
        crit.MinPrice = request.MinPrice ?? 0;
        crit.MaxPrice = request.MaxPrice ?? 0;
        crit.Location = request.Location ?? string.Empty;
        await _context.SaveChangesAsync(cancellationToken);
        return new UpdateSearchCriteriaResponse { Id = crit.Id };
    }
} 