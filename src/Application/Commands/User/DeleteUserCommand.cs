using System;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Application.Commands.User;

public class DeleteUserCommand : IRequest<DeleteUserResponse>
{
    public Guid Id { get; set; }
}

public class DeleteUserResponse
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponse>
{
    private readonly UserManager<Domain.Entities.User> _userManager;

    public DeleteUserCommandHandler(UserManager<Domain.Entities.User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<DeleteUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user is null)
        {
            throw new KeyNotFoundException($"User with ID {request.Id} not found.");
        }

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to delete user: {errors}");
        }

        return new DeleteUserResponse { Id = request.Id, IsDeleted = true };
    }
} 