using System;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Application.Commands.User;

public class CreateUserCommand : IRequest<CreateUserResponse>
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class CreateUserResponse
{
    public Guid Id { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly UserManager<Domain.Entities.User> _userManager;

    public CreateUserCommandHandler(UserManager<Domain.Entities.User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new Domain.Entities.User
        {
            UserName = request.Name,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to create user: {errors}");
        }

        return new CreateUserResponse { Id = user.Id };
    }
} 