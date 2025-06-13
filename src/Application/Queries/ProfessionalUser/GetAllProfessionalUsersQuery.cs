using Infrastructure.Persistences;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.ProfessionalUser;

public class GetAllProfessionalUsersQuery : IRequest<GetAllProfessionalUsersResponse>
{
    public string? SearchTerm { get; set; }
    public int? AgencyId { get; set; }
}

public class GetAllProfessionalUsersResponse
{
    public List<GetAllProfessionalUsersResponseItem> Users { get; set; } = [];
    public int TotalCount { get; set; }
}

public class GetAllProfessionalUsersResponseItem
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public int AgencyId { get; set; }
}

public class GetAllProfessionalUsersQueryHandler : IRequestHandler<GetAllProfessionalUsersQuery, GetAllProfessionalUsersResponse>
{
    private readonly ImmotechDbContext _context;

    public GetAllProfessionalUsersQueryHandler(ImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<GetAllProfessionalUsersResponse> Handle(GetAllProfessionalUsersQuery request, CancellationToken cancellationToken)
    {
        var users = _context.ProfessionalUsers.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            users = users.Where(u => u.UserName.Contains(request.SearchTerm) || u.Email.Contains(request.SearchTerm));
        }
        if (request.AgencyId.HasValue)
        {
            users = users.Where(u => u.AgencyId == request.AgencyId.Value);
        }

        var list = await users.Select(u => new GetAllProfessionalUsersResponseItem
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email,
            AgencyId = u.AgencyId
        }).ToListAsync(cancellationToken);

        return new GetAllProfessionalUsersResponse { Users = list, TotalCount = list.Count };
    }
} 