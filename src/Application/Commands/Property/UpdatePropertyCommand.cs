using System;
using Domain.Entities;
using Infrastructure.Persistences;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Property;

public class UpdatePropertyCommand : IRequest<UpdatePropertyResponse>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Address Address { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public PropertyStatus Status { get; set; }
    public int? AgencyId { get; set; }
}

public class UpdatePropertyResponse
{
    public Guid Id { get; set; }
}

public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, UpdatePropertyResponse>
{
    private readonly ImmotechDbContext _context;

    public UpdatePropertyCommandHandler(ImmotechDbContext context)
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
        property.Status = request.Status;
        property.AgencyId = request.AgencyId;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdatePropertyResponse { Id = property.Id };
    }
} 