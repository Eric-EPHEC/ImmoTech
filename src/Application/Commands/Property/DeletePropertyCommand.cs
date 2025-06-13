using System;
using Infrastructure.Persistences;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Property;

public class DeletePropertyCommand : IRequest<DeletePropertyResponse>
{
    public Guid Id { get; set; }
}

public class DeletePropertyResponse
{
    public Guid Id { get; set; }
   
}

public class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand, DeletePropertyResponse>
{
    private readonly ImmotechDbContext _context;

    public DeletePropertyCommandHandler(ImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<DeletePropertyResponse> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
    {

        await _context.Properties.Where(p => p.Id == request.Id).ExecuteDeleteAsync(cancellationToken);

        return new DeletePropertyResponse { Id = request.Id};
    }
} 