using Domain.Entities;
using Infrastructure.Persistences;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Agency;

public class UpdateAgencyCommand : IRequest<UpdateAgencyResponse>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public Address? Address { get; set; }
    public string? ContactEmail { get; set; }
}

public class UpdateAgencyResponse
{
    public int Id { get; set; }
}

public class UpdateAgencyCommandHandler : IRequestHandler<UpdateAgencyCommand, UpdateAgencyResponse>
{
    private readonly ImmotechDbContext _context;

    public UpdateAgencyCommandHandler(ImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateAgencyResponse> Handle(UpdateAgencyCommand request, CancellationToken cancellationToken)
    {
        var agency = await _context.Agencies.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (agency is null)
        {
            throw new KeyNotFoundException($"Agency with ID {request.Id} not found.");
        }

        agency.Name = request.Name;
        agency.Address = request.Address;
        agency.ContactEmail = request.ContactEmail;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateAgencyResponse { Id = agency.Id };
    }
} 