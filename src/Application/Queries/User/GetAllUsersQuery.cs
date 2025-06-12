using System.Collections.Generic;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.User
{
    public class GetAllUsersQuery : IRequest<GetAllUsersResponse>
    {
        public string? SearchTerm { get; set; }
        public bool? EmailConfirmed { get; set; }
        public bool? LockoutEnabled { get; set; }
    }

    public class GetAllUsersResponse
    {
        public List<GetAllUsersResponseItem> Users { get; set; } = [];
        public int TotalCount { get; set; }
    }

    public class GetAllUsersResponseItem
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, GetAllUsersResponse>
    {
        private readonly UserManager<Domain.Entities.User> _userManager;

        public GetAllUsersQueryHandler(UserManager<Domain.Entities.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetAllUsersResponse> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = _userManager.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                users = users.Where(u => 
                    u.UserName.Contains(request.SearchTerm) || 
                    u.Email.Contains(request.SearchTerm));
            }

            if (request.EmailConfirmed.HasValue)
            {
                users = users.Where(u => u.EmailConfirmed == request.EmailConfirmed.Value);
            }

            if (request.LockoutEnabled.HasValue)
            {
                users = users.Where(u => u.LockoutEnabled == request.LockoutEnabled.Value);
            }

            var userList = await users
                .Select(u => new GetAllUsersResponseItem
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    EmailConfirmed = u.EmailConfirmed,
                    PhoneNumberConfirmed = u.PhoneNumberConfirmed,
                    TwoFactorEnabled = u.TwoFactorEnabled,
                    LockoutEnd = u.LockoutEnd,
                    LockoutEnabled = u.LockoutEnabled,
                    AccessFailedCount = u.AccessFailedCount
                })
                .ToListAsync(cancellationToken);

            return new GetAllUsersResponse
            {
                Users = userList,
                TotalCount = userList.Count
            };
        }
    }
} 