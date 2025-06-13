using Domain.Entities;
using Infrastructure.Persistences;
using MediatR;

namespace Application.Commands.ProfessionalUser;

public class CreateProfessionalUserCommand : IRequest<CreateProfessionalUserResponse>
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public int AgencyId { get; set; }
}

public class CreateProfessionalUserResponse
{
    public Guid Id { get; set; }
}

public class CreateProfessionalUserCommandHandler : IRequestHandler<CreateProfessionalUserCommand, CreateProfessionalUserResponse>
{
    private readonly ImmotechDbContext _context;

    public CreateProfessionalUserCommandHandler(ImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<CreateProfessionalUserResponse> Handle(CreateProfessionalUserCommand request, CancellationToken cancellationToken)
    {
        var proUser = new Domain.Entities.ProfessionalUser
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            Email = request.Email,
            AgencyId = request.AgencyId
        };

        _context.ProfessionalUsers.Add(proUser);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateProfessionalUserResponse { Id = proUser.Id };
    }
} 