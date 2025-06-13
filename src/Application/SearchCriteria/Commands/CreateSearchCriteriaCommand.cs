using MediatR;
using System;

namespace Application.Commands.SearchCriteria;

public class CreateSearchCriteriaCommand : IRequest<int>
{
    public string Keywords { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public string Location { get; set; }
    public Guid UserId { get; set; }
} 