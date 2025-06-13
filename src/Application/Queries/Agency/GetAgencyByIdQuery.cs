using Domain.Entities;

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Agency
{
    public class GetAgencyByIdQuery : IRequest<GetAgencyByIdResponse>
    {
        public int Id { get; set; }
    }

    public class GetAgencyByIdResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Address? Address { get; set; }
        public string? ContactEmail { get; set; }
    }

    public class GetAgencyByIdQueryHandler : IRequestHandler<GetAgencyByIdQuery, GetAgencyByIdResponse>
    {
        private readonly IImmotechDbContext _context;

        public GetAgencyByIdQueryHandler(IImmotechDbContext context)
        {
            _context = context;
        }

        public async Task<GetAgencyByIdResponse> Handle(GetAgencyByIdQuery request, CancellationToken cancellationToken)
        {
            var agency = await _context.Agencies
                .AsNoTracking()
                .Where(a => a.Id == request.Id)
                .Select(a => new GetAgencyByIdResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Address = a.Address,
                    ContactEmail = a.ContactEmail
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (agency == null)
            {
                throw new KeyNotFoundException($"Agency with ID {request.Id} not found.");
            }

            return agency;
        }
    }
} 