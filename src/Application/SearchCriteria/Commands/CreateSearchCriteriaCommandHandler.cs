using Application.Common;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.SearchCriteria;

public class CreateSearchCriteriaCommandHandler : IRequestHandler<CreateSearchCriteriaCommand, int>
{
    private readonly IImmotechDbContext _context;

    public CreateSearchCriteriaCommandHandler(IImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateSearchCriteriaCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.SearchCriteria
        {
            Keywords = request.Keywords,
            MinPrice = request.MinPrice,
            MaxPrice = request.MaxPrice,
            Location = request.Location,
            UserId = request.UserId
        };

        _context.SearchCriterias.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
