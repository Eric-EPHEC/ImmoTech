using System;
using Domain.Entities;

using MediatR;

namespace Application.Commands.Property;

public class CreatePropertyCommand : IRequest<CreatePropertyResponse>
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required Address Address { get; set; }
    public required string Location { get; set; }
    public decimal Price { get; set; }
    public int Bedrooms { get; set; }
    public int SurfaceArea { get; set; }
    public Domain.Entities.PropertyType Type { get; set; }
    public int? AgencyId { get; set; }
    public PropertyBidType BidType { get; set; } = PropertyBidType.Sale;
}

public class CreatePropertyResponse
{
    public Guid Id { get; set; }
}

public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, CreatePropertyResponse>
{
    private readonly IImmotechDbContext _context;

    public CreatePropertyCommandHandler(IImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<CreatePropertyResponse> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        var property = new Domain.Entities.Property
        {
            Title = request.Title,
            Description = request.Description,
            Address = request.Address,
            Location = request.Location,
            Price = request.Price,
            Bedrooms = request.Bedrooms,
            SurfaceArea = request.SurfaceArea,
            Status = PropertyStatus.Available,
            Type = request.Type,
            CreatedDate = DateTimeOffset.UtcNow,
            AgencyId = request.AgencyId,
            BidType = request.BidType
        };

        _context.Properties.Add(property);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreatePropertyResponse { Id = property.Id };
    }
} 