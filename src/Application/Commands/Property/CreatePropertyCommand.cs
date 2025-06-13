using System;
using Domain.Entities;
using Infrastructure.Persistences;
using MediatR;

namespace Application.Commands.Property;

public class CreatePropertyCommand : IRequest<CreatePropertyResponse>
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required Address Address { get; set; }
    public required string Location { get; set; }
    public decimal Price { get; set; }
    public int? AgencyId { get; set; }
}

public class CreatePropertyResponse
{
    public Guid Id { get; set; }
}

public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, CreatePropertyResponse>
{
    private readonly ImmotechDbContext _context;

    public CreatePropertyCommandHandler(ImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<CreatePropertyResponse> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        var property = new Domain.Entities.Property
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Address = request.Address,
            Location = request.Location,
            Price = request.Price,
            Status = PropertyStatus.Available,
            CreatedDate = DateTimeOffset.UtcNow,
            AgencyId = request.AgencyId
        };

        await _context.Properties.AddAsync(property, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreatePropertyResponse { Id = property.Id };
    }
} 