using Immotech.Api.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;

namespace Immotech.Api.Controllers;

[ApiController]
[Route("auth")] // e.g., /auth/register, /auth/login
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtTokenGenerator _tokenGenerator;

    // Inject UserManager for user creation, SignInManager for password checks, and our JWT generator.
    public AuthController(
        UserManager<User> userManager, 
        IJwtTokenGenerator tokenGenerator, 
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _tokenGenerator = tokenGenerator;
        _signInManager = signInManager;
    }

    // DTO for registration request
    public record RegisterRequest(string Email, string Password);
    // DTO for login request
    public record LoginRequest(string Email, string Password);
    // DTO for auth response containing the token
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

    // This endpoint handles user login with email and password.
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        // Find the user by their email address.
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            // If user is not found, return unauthorized.
            return Unauthorized("Invalid credentials");
        }

        // Check if the provided password is correct for the user.
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            // If password check fails, return unauthorized.
            return Unauthorized("Invalid credentials");
        }

        // If credentials are valid, generate a JWT for the user.
        var token = _tokenGenerator.GenerateToken(user);
        return Ok(new AuthResponse(token));
    }
} 