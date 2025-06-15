using Immotech.Api.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;

namespace Immotech.Api.Controllers;

[ApiController]
[Route("auth")] // /auth/register
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public AuthController(UserManager<User> userManager, IJwtTokenGenerator tokenGenerator)
    {
        _userManager = userManager;
        _tokenGenerator = tokenGenerator;
    }

    public record RegisterRequest(string Email, string Password);
    public record AuthResponse(string Token);

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        var existing = await _userManager.FindByEmailAsync(request.Email);
        if (existing is not null)
            return Conflict("Email already registered");

        var user = new User { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors.Select(e => e.Description));

        var token = _tokenGenerator.GenerateToken(user);
        return Ok(new AuthResponse(token));
    }
} 