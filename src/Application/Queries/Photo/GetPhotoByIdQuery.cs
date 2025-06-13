using Domain.Entities;
using Infrastructure.Persistences;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Photo;

public class GetPhotoByIdQuery : IRequest<GetPhotoByIdResponse>
{
    public int Id { get; set; }
}

public class GetPhotoByIdResponse
{
    public int Id { get; set; }
    public string? Url { get; set; }
    public DateTimeOffset UploadedAt { get; set; }
}

public class GetPhotoByIdQueryHandler : IRequestHandler<GetPhotoByIdQuery, GetPhotoByIdResponse>
{
    private readonly IImmotechDbContext _context;

    public GetPhotoByIdQueryHandler(IImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<GetPhotoByIdResponse> Handle(GetPhotoByIdQuery request, CancellationToken cancellationToken)
    {
        var photo = await _context.Photos
            .AsNoTracking()
            .Where(p => p.Id == request.Id)
            .Select(p => new GetPhotoByIdResponse
            {
                Id = p.Id,
                Url = p.Url,
                UploadedAt = p.UploadedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (photo is null)
        {
            throw new KeyNotFoundException($"Photo with ID {request.Id} not found.");
        }

        return photo;
    }
} 