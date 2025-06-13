
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
}

public class DeletePhotoCommandHandler : IRequestHandler<DeletePhotoCommand, DeletePhotoResponse>
{
    private readonly IImmotechDbContext _context;

    public DeletePhotoCommandHandler(IImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<DeletePhotoResponse> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
    {
        var affectedRows = await _context.Photos
            .Where(p => p.Id == request.Id)
            .ExecuteDeleteAsync(cancellationToken);

        if (affectedRows == 0)
        {
            throw new KeyNotFoundException($"Photo with ID {request.Id} not found.");
        }

        return new DeletePhotoResponse { Id = request.Id };
    }
} 