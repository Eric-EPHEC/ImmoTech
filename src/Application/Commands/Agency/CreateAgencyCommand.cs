using Domain.Entities;
using Infrastructure.Persistences;
using MediatR;

namespace Application.Commands.Agency;

public class CreateAgencyCommand : IRequest<CreateAgencyResponse>
{
    public required string Name { get; set; }
    public required Address Address { get; set; }
    public required string ContactEmail { get; set; }
}

public class CreateAgencyResponse
{
    public int Id { get; set; }
}

public class CreateAgencyCommandHandler : IRequestHandler<CreateAgencyCommand, CreateAgencyResponse>
{
    private readonly ImmotechDbContext _context;

    public CreateAgencyCommandHandler(ImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<CreateAgencyResponse> Handle(CreateAgencyCommand request, CancellationToken cancellationToken)
    {
        var agency = new Domain.Entities.Agency
        {
            Name = request.Name,
            Address = request.Address,
            ContactEmail = request.ContactEmail
        };

        _context.Agencies.Add(agency);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateAgencyResponse { Id = agency.Id };
    }
} 