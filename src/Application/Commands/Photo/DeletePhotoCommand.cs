using Infrastructure.Persistences;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Photo;

public class DeletePhotoCommand : IRequest<DeletePhotoResponse>
{
    public int Id { get; set; }
}

public class DeletePhotoResponse
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
}

public class DeletePhotoCommandHandler : IRequestHandler<DeletePhotoCommand, DeletePhotoResponse>
{
    private readonly ImmotechDbContext _context;

    public DeletePhotoCommandHandler(ImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<DeletePhotoResponse> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
    {
        var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (photo is null)
        {
            throw new KeyNotFoundException($"Photo with ID {request.Id} not found.");
        }

        _context.Photos.Remove(photo);
        await _context.SaveChangesAsync(cancellationToken);

        return new DeletePhotoResponse { Id = request.Id, IsDeleted = true };
    }
} 