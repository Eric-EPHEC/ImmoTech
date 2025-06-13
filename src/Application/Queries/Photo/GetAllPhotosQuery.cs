using Domain.Entities;
using Infrastructure.Persistences;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Photo;

public class GetAllPhotosQuery : IRequest<GetAllPhotosResponse>
{
    // Optional search filters
    public string? UrlSearchTerm { get; set; }
    public DateTimeOffset? UploadedAfter { get; set; }
    public DateTimeOffset? UploadedBefore { get; set; }
}

public class GetAllPhotosResponse
{
    public List<GetAllPhotosResponseItem> Photos { get; set; } = [];
    public int TotalCount { get; set; }
}

public class GetAllPhotosResponseItem
{
    public int Id { get; set; }
    public string? Url { get; set; }
    public DateTimeOffset UploadedAt { get; set; }
}

public class GetAllPhotosQueryHandler : IRequestHandler<GetAllPhotosQuery, GetAllPhotosResponse>
{
    private readonly ImmotechDbContext _context;

    public GetAllPhotosQueryHandler(ImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<GetAllPhotosResponse> Handle(GetAllPhotosQuery request, CancellationToken cancellationToken)
    {
        var photos = _context.Photos.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.UrlSearchTerm))
        {
            photos = photos.Where(p => p.Url.Contains(request.UrlSearchTerm));
        }

        if (request.UploadedAfter.HasValue)
        {
            photos = photos.Where(p => p.UploadedAt >= request.UploadedAfter.Value);
        }

        if (request.UploadedBefore.HasValue)
        {
            photos = photos.Where(p => p.UploadedAt <= request.UploadedBefore.Value);
        }

        var photoList = await photos.Select(p => new GetAllPhotosResponseItem
        {
            Id = p.Id,
            Url = p.Url,
            UploadedAt = p.UploadedAt
        }).ToListAsync(cancellationToken);

        return new GetAllPhotosResponse
        {
            Photos = photoList,
            TotalCount = photoList.Count
        };
    }
} 