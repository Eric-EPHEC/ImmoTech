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
    public bool IsDeleted { get; set; }
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
        var property = await _context.Properties.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (property is null)
        {
            throw new KeyNotFoundException($"Property with ID {request.Id} not found.");
        }

        _context.Properties.Remove(property);
        await _context.SaveChangesAsync(cancellationToken);

        return new DeletePropertyResponse { Id = request.Id, IsDeleted = true };
    }
} 