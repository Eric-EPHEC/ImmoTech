using Infrastructure.Persistences;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.ProfessionalUser;

public class DeleteProfessionalUserCommand : IRequest<DeleteProfessionalUserResponse>
{
    public Guid Id { get; set; }
}

public class DeleteProfessionalUserResponse
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
}

public class DeleteProfessionalUserCommandHandler : IRequestHandler<DeleteProfessionalUserCommand, DeleteProfessionalUserResponse>
{
    private readonly ImmotechDbContext _context;

    public DeleteProfessionalUserCommandHandler(ImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<DeleteProfessionalUserResponse> Handle(DeleteProfessionalUserCommand request, CancellationToken cancellationToken)
    {
        var proUser = await _context.ProfessionalUsers.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
        if (proUser is null)
        {
            throw new KeyNotFoundException($"ProfessionalUser with ID {request.Id} not found.");
        }

        _context.ProfessionalUsers.Remove(proUser);
        await _context.SaveChangesAsync(cancellationToken);

        return new DeleteProfessionalUserResponse { Id = request.Id, IsDeleted = true };
    }
} 