using System;
using Domain.Entities;

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Property;

public class UpdatePropertyCommand : IRequest<UpdatePropertyResponse>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public required Address Address { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Bedrooms { get; set; }
    public int SurfaceArea { get; set; }
    public PropertyStatus Status { get; set; }
    public Domain.Entities.PropertyType Type { get; set; }
    public int? AgencyId { get; set; }
    public PropertyBidType BidType { get; set; } = PropertyBidType.Sale;
}

public class UpdatePropertyResponse
{
    public Guid Id { get; set; }
}

public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, UpdatePropertyResponse>
{
    private readonly IImmotechDbContext _context;

    public UpdatePropertyCommandHandler(IImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<UpdatePropertyResponse> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
    {
        var property = await _context.Properties.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (property is null)
        {
            throw new KeyNotFoundException($"Property with ID {request.Id} not found.");
        }

        property.Title = request.Title;
        property.Description = request.Description;
        property.Address = request.Address;
        property.Location = request.Location;
        property.Price = request.Price;
        property.Bedrooms = request.Bedrooms;
        property.SurfaceArea = request.SurfaceArea;
        property.Status = request.Status;
        property.Type = request.Type;
        property.AgencyId = request.AgencyId;
        property.BidType = request.BidType;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdatePropertyResponse { Id = property.Id };
    }
} 