using Domain.Entities;
using Infrastructure.Persistences;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Photo;

public class UpdatePhotoCommand : IRequest<UpdatePhotoResponse>
{
    public int Id { get; set; }
    public string? Url { get; set; }
    public bool? IsMain { get; set; }
}

public class UpdatePhotoResponse
{
    public int Id { get; set; }
}

public class UpdatePhotoCommandHandler : IRequestHandler<UpdatePhotoCommand, UpdatePhotoResponse>
{
    private readonly ImmotechDbContext _context;

    public UpdatePhotoCommandHandler(ImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<UpdatePhotoResponse> Handle(UpdatePhotoCommand request, CancellationToken cancellationToken)
    {
        var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (photo is null)
        {
            throw new KeyNotFoundException($"Photo with ID {request.Id} not found.");
        }

        if (request.Url is not null)
        {
            photo.Url = request.Url;
        }

        if (request.IsMain.HasValue)
        {
            photo.IsMain = request.IsMain.Value;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdatePhotoResponse { Id = photo.Id };
    }
} 