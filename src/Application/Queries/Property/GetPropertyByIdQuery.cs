using System;
using Domain.Entities;
using Infrastructure.Persistences;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Property
{
    public class GetPropertyByIdQuery : IRequest<GetPropertyByIdResponse>
    {
        public Guid Id { get; set; }
    }

    public class GetPropertyByIdResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public PropertyStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int? AgencyId { get; set; }
        public AgencyResponse Agency { get; set; }
        public List<PhotoResponse> Photos { get; set; }
    }

    public class AgencyResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactEmail { get; set; }
    }

    public class PhotoResponse
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public DateTimeOffset UploadedAt { get; set; }
    }

    public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, GetPropertyByIdResponse>
    {
        private readonly ImmotechDbContext _context;

        public GetPropertyByIdQueryHandler(ImmotechDbContext context)
        {
            _context = context;
        }

        public async Task<GetPropertyByIdResponse> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var property = await _context.Properties
                .AsNoTracking()
                .Where(p => p.Id == request.Id)
                .Select(p => new GetPropertyByIdResponse
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Address = p.Address,
                    Location = p.Location,
                    Price = p.Price,
                    Status = p.Status,
                    CreatedDate = p.CreatedDate,
                    AgencyId = p.AgencyId,
                    Agency = p.Agency != null ? new AgencyResponse
                    {
                        Id = p.Agency.Id,
                        Name = p.Agency.Name,
                        ContactEmail = p.Agency.ContactEmail
                    } : null,
                    Photos = p.Photos.Select(photo => new PhotoResponse
                    {
                        Id = photo.Id,
                        Url = photo.Url,
                        UploadedAt = photo.UploadedAt
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (property == null)
            {
                throw new KeyNotFoundException($"Property with ID {request.Id} not found.");
            }

            return property;
        }
    }
} 