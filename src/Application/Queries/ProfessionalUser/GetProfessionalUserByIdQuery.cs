using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.ProfessionalUser;

public class GetProfessionalUserByIdQuery : IRequest<GetProfessionalUserByIdResponse>
{
    public Guid Id { get; set; }
}

public class GetProfessionalUserByIdResponse
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public int AgencyId { get; set; }
}

public class GetProfessionalUserByIdQueryHandler : IRequestHandler<GetProfessionalUserByIdQuery, GetProfessionalUserByIdResponse>
{
    private readonly IImmotechDbContext _context;

    public GetProfessionalUserByIdQueryHandler(IImmotechDbContext context)
    {
        _context = context;
    }

    public async Task<GetProfessionalUserByIdResponse> Handle(GetProfessionalUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.ProfessionalUsers.AsNoTracking()
            .Where(u => u.Id == request.Id)
            .Select(u => new GetProfessionalUserByIdResponse
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                AgencyId = u.AgencyId
            }).FirstOrDefaultAsync(cancellationToken);
        if (user is null) throw new KeyNotFoundException($"ProfessionalUser with ID {request.Id} not found.");
        return user;
    }
} 