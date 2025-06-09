using System.Collections.Generic;
using Domain.Entities;

namespace Application.Queries.User
{
    public class GetAllUsersQuery
    {
        public UserRole? Role { get; set; }
    }

    public class UserListDto
    {
        public List<UserDto> Users { get; set; }
        public int TotalCount { get; set; }
    }
} 