using Infrastructure.Persistences;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.ProfessionalUser;

public class UpdateProfessionalUserCommand : IRequest<UpdateProfessionalUserResponse>
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public int? AgencyId { get; set; }
}

public class UpdateProfessionalUserResponse
{
    public Guid Id { get; set; }
}

public class UpdateProfessionalUserCommandHandler : IRequestHandler<UpdateProfessionalUserCommand, UpdateProfessionalUserResponse>
{
    private readonly ImmotechDbContext _context;

    public UpdateProfessionalUserCommandHandler(ImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateProfessionalUserResponse> Handle(UpdateProfessionalUserCommand request, CancellationToken cancellationToken)
    {
        var proUser = await _context.ProfessionalUsers.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
        if (proUser is null)
        {
            throw new KeyNotFoundException($"ProfessionalUser with ID {request.Id} not found.");
        }

        if (request.UserName is not null) proUser.UserName = request.UserName;
        if (request.Email is not null) proUser.Email = request.Email;
        if (request.AgencyId.HasValue) proUser.AgencyId = request.AgencyId.Value;

        await _context.SaveChangesAsync(cancellationToken);
        return new UpdateProfessionalUserResponse { Id = proUser.Id };
    }
} 