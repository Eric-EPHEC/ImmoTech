using System.Collections.Generic;
using Domain.Entities;

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Property
{
    public class GetAllPropertiesQuery : IRequest<GetAllPropertiesResponse>
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Location { get; set; }
        public PropertyStatus? Status { get; set; }
        public int? AgencyId { get; set; }
    }

    public class GetAllPropertiesResponse
    {
        public List<GetAllPropertiesResponseItem> Properties { get; set; }=[];
        public int TotalCount { get; set; }
    }

    public class GetAllPropertiesResponseItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public int Bedrooms { get; set; }
        public decimal SurfaceArea { get; set; }
        public PropertyStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int? AgencyId { get; set; }
        public AgencyResponse Agency { get; set; }
        public List<PhotoResponse> Photos { get; set; }
    }


    public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, GetAllPropertiesResponse>
    {
        private readonly IImmotechDbContext _context;

        public GetAllPropertiesQueryHandler(IImmotechDbContext context)
        {
            _context = context;
        }

        public async Task<GetAllPropertiesResponse> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
        {
            // Ignore global filters for the unauthenticated user to see all properties
            var query = _context.Properties.AsNoTracking().IgnoreQueryFilters();

            if (request.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= request.MaxPrice.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Location))
            {
                query = query.Where(p => p.Location.Contains(request.Location));
            }

            if (request.Status.HasValue)
            {
                query = query.Where(p => p.Status == request.Status.Value);
            }

            if (request.AgencyId.HasValue)
            {
                query = query.Where(p => p.AgencyId == request.AgencyId.Value);
            }
            //Possiblité d'améliorer les performances du filtre en utilisant du full text search.
            var properties = await query
                .Select(p => new GetAllPropertiesResponseItem
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Address = p.Address,
                    Location = p.Location,
                    Price = p.Price,
                    Bedrooms = p.Bedrooms,
                    SurfaceArea = p.SurfaceArea,
                    Status = p.Status,
                    CreatedDate = p.CreatedDate,
                    AgencyId = p.AgencyId,
                    Photos = p.Photos.Select(ph => new PhotoResponse { Id = ph.Id, Url = ph.Url, UploadedAt = ph.UploadedAt, IsMain = ph.IsMain }).ToList(),
                    Agency = p.Agency != null ? new AgencyResponse { Id = p.Agency.Id, Name = p.Agency.Name, ContactEmail = p.Agency.ContactEmail, LogoUrl = p.Agency.LogoUrl } : null,
                })
                .ToListAsync(cancellationToken);

            return new GetAllPropertiesResponse
            {
                Properties = properties,
                TotalCount = properties.Count
            };
        }
    }
} 