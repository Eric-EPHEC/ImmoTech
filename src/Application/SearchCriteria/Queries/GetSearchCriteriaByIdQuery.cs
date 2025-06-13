using MediatR;

namespace Application.Queries.SearchCriteria;

public class GetSearchCriteriaByIdQuery : IRequest<Domain.Entities.SearchCriteria>
{
    public int Id { get; set; }
} 