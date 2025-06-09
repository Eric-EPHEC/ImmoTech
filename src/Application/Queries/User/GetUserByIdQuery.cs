using System;
using Domain.Entities;

namespace Application.Queries.User
{
    public class GetUserByIdQuery
    {
        public int Id { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public DateTime RegisterDate { get; set; }
    }
} 