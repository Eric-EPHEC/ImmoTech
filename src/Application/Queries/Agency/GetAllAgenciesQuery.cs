using System.Collections.Generic;
using Domain.Entities;

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Agency
{
    public class GetAllAgenciesQuery : IRequest<GetAllAgenciesResponse>
    {
        // Optional search term to filter agencies by name or contact email
        public string? SearchTerm { get; set; }
    }

    public class GetAllAgenciesResponse
    {
        public List<GetAllAgenciesResponseItem> Agencies { get; set; } = [];
        public int TotalCount { get; set; }
    }

    public class GetAllAgenciesResponseItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Address? Address { get; set; }
        public string? ContactEmail { get; set; }
    }

    public class GetAllAgenciesQueryHandler : IRequestHandler<GetAllAgenciesQuery, GetAllAgenciesResponse>
    {
        private readonly IImmotechDbContext _context;

        public GetAllAgenciesQueryHandler(IImmotechDbContext context)
        {
            _context = context;
        }

        public async Task<GetAllAgenciesResponse> Handle(GetAllAgenciesQuery request, CancellationToken cancellationToken)
        {
            var agencies = _context.Agencies.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                agencies = agencies.Where(a => a.Name.Contains(request.SearchTerm) || a.ContactEmail.Contains(request.SearchTerm));
            }

            var agencyList = await agencies.Select(a => new GetAllAgenciesResponseItem
            {
                Id = a.Id,
                Name = a.Name,
                Address = a.Address,
                ContactEmail = a.ContactEmail,
            }).ToListAsync(cancellationToken);

            return new GetAllAgenciesResponse
            {
                Agencies = agencyList,
                TotalCount = agencyList.Count
            };
        }
    }
} 