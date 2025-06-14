using Domain.Entities;  
using Microsoft.AspNetCore.Identity;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.ProfessionalUser;

public class CreateProfessionalUserCommand : IRequest<CreateProfessionalUserResponse>
{
    [Required]
    public required string UserName { get; set; }
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    public int AgencyId { get; set; }
    [Required]
    public required string Password { get; set; }
}

public class CreateProfessionalUserResponse
{
    public Guid Id { get; set; }
}

public class CreateProfessionalUserCommandHandler : IRequestHandler<CreateProfessionalUserCommand, CreateProfessionalUserResponse>
{
    private readonly UserManager<Domain.Entities.User> _userManager;

    public CreateProfessionalUserCommandHandler(UserManager<Domain.Entities.User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CreateProfessionalUserResponse> Handle(CreateProfessionalUserCommand request, CancellationToken cancellationToken)
    {
        var proUser = new Domain.Entities.ProfessionalUser
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            Email = request.Email,
            AgencyId = request.AgencyId
        };

        // CreateAsync validates username, email and password using the configured validators, then persists the user.
        var result = await _userManager.CreateAsync(proUser, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ValidationException($"Unable to create ProfessionalUser: {errors}");
        }
        

        return new CreateProfessionalUserResponse { Id = proUser.Id };
    }
} 