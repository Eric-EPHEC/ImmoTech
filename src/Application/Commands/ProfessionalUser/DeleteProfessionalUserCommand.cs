
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
   
}

public class DeleteProfessionalUserCommandHandler : IRequestHandler<DeleteProfessionalUserCommand, DeleteProfessionalUserResponse>
{
    private readonly IImmotechDbContext _context;

    public DeleteProfessionalUserCommandHandler(IImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<DeleteProfessionalUserResponse> Handle(DeleteProfessionalUserCommand request, CancellationToken cancellationToken)
    {
        await _context.ProfessionalUsers.Where(u => u.Id == request.Id).ExecuteDeleteAsync(cancellationToken);

        return new DeleteProfessionalUserResponse { Id = request.Id};
    }
} 