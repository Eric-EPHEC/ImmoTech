using System;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Application.Commands.User;

public class UpdateUserCommand : IRequest<UpdateUserResponse>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Password { get; set; }
}

public class UpdateUserResponse
{
    public Guid Id { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
{
    private readonly UserManager<Domain.Entities.User> _userManager;

    public UpdateUserCommandHandler(UserManager<Domain.Entities.User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user is null)
        {
            throw new KeyNotFoundException($"User with ID {request.Id} not found.");
        }

        user.UserName = request.Name;
        user.Email = request.Email;

        // Update password if provided
        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResult = await _userManager.ResetPasswordAsync(user, token, request.Password);

            if (!passwordResult.Succeeded)
            {
                var errors = string.Join("; ", passwordResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to update password: {errors}");
            }
        }

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to update user: {errors}");
        }

        return new UpdateUserResponse { Id = user.Id };
    }
} 