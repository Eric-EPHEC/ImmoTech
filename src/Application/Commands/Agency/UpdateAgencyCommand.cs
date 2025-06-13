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
    public string? LogoUrl { get; set; }
}

public class UpdateAgencyResponse
{
    public int Id { get; set; }
}

public class UpdateAgencyCommandHandler : IRequestHandler<UpdateAgencyCommand, UpdateAgencyResponse>
{
    private readonly IImmotechDbContext _context;

    public UpdateAgencyCommandHandler(IImmotechDbContext context)
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

        if (request.Name is not null)
        {
            agency.Name = request.Name;
        }

        if (request.Address is not null)
        {
            agency.Address = request.Address;
        }

        if (request.ContactEmail is not null)
        {
            agency.ContactEmail = request.ContactEmail;
        }

        if (request.LogoUrl is not null)
        {
            agency.LogoUrl = request.LogoUrl;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateAgencyResponse { Id = agency.Id };
    }
} 