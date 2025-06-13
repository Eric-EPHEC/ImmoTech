using Domain.Entities;
using Infrastructure.Persistences;
using MediatR;

namespace Application.Commands.Photo;

public class CreatePhotoCommand : IRequest<CreatePhotoResponse>
{
    public required string Url { get; set; }
}

public class CreatePhotoResponse
{
    public int Id { get; set; }
}

public class CreatePhotoCommandHandler : IRequestHandler<CreatePhotoCommand, CreatePhotoResponse>
{
    private readonly ImmotechDbContext _context;

    public CreatePhotoCommandHandler(ImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<CreatePhotoResponse> Handle(CreatePhotoCommand request, CancellationToken cancellationToken)
    {
        var photo = new Domain.Entities.Photo
        {
            Url = request.Url,
            UploadedAt = DateTimeOffset.UtcNow
        };

        _context.Photos.Add(photo);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreatePhotoResponse { Id = photo.Id };
    }
} 