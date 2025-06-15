using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Domain.Entities;
using Immotech.Api.Common;

namespace Immotech.Api.Controllers;

// Controller responsible for kicking off & handling social auth
[ApiController]
[Route("auth/external")] // e.g. /auth/external/google
public class ExternalAuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _config;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public ExternalAuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config, IJwtTokenGenerator tokenGenerator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
        _tokenGenerator = tokenGenerator;
    }

    // This action initiates the external login flow
    // GET /auth/external/{provider}
    [HttpGet("{provider}")]
    public IActionResult ExternalLogin([FromRoute] string provider, [FromQuery] string? returnUrl)
    {
        // We set the callback URL that the external provider will redirect to after authentication
        var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "ExternalAuth", new { returnUrl });
        // We configure the authentication properties, including the redirect URL
        var props = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        // We challenge the user to authenticate with the external provider
        return Challenge(props, provider);
    }

    // This endpoint handles the callback from the external provider
    // GET /auth/external/callback
    [HttpGet("callback")]
    public async Task<IActionResult> ExternalLoginCallback([FromQuery] string? returnUrl)
    {
        // Use a default return URL if none is provided
        returnUrl = string.IsNullOrWhiteSpace(returnUrl) ? "/external-callback" : returnUrl;

        // Retrieve login information from the external provider
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            // If we can't get login info, it's an error. Redirect to the SPA's login page with an error message.
            return RedirectToSpa(returnUrl, error: "Failed to get external login info.");
        }

        // Attempt to sign in the user with the external login provider information.
        // This checks if the user has already linked this external login to their account.
        var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        
        User? user;

        if (signInResult.Succeeded)
        {
            // If sign-in was successful, find the user.
            user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user is null)
            {
                // This is an unexpected state. If sign-in succeeded, a user should exist.
                return RedirectToSpa(returnUrl, error: "External user not found after successful sign-in.");
            }
        }
        else
        {
            // If the user does not have an account, we create a new one.
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
            {
                // We require an email from the external provider to create an account.
                return RedirectToSpa(returnUrl, error: "Email claim not received from external provider.");
            }

            // Check if a user with this email already exists
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                // If a user with this email exists, link the external login to their account.
                var addLoginResult = await _userManager.AddLoginAsync(existingUser, info);
                if (!addLoginResult.Succeeded)
                {
                    return RedirectToSpa(returnUrl, error: "Failed to add external login to existing user.");
                }
                user = existingUser;
            }
            else
            {
                // If no user exists, create a new user account.
                // We mark the email as confirmed since it comes from a trusted external provider.
                user = new User { UserName = email, Email = email, EmailConfirmed = true };
                var createUserResult = await _userManager.CreateAsync(user);
                if (!createUserResult.Succeeded)
                {
                    var error = createUserResult.Errors.FirstOrDefault()?.Description ?? "Failed to create user.";
                    return RedirectToSpa(returnUrl, error: error);
                }

                // Link the external login to the newly created user account.
                var addLoginResult = await _userManager.AddLoginAsync(user, info);
                if (!addLoginResult.Succeeded)
                {
                    var error = addLoginResult.Errors.FirstOrDefault()?.Description ?? "Failed to add external login.";
                    return RedirectToSpa(returnUrl, error: error);
                }
            }
        }

        // At this point, we have a valid user. We generate a JWT for them.
        var token = _tokenGenerator.GenerateToken(user);

        // Redirect back to the SPA, passing the token as a query parameter.
        return RedirectToSpaWithToken(returnUrl, token);
    }
    
    // Helper method to create the redirect URL for the SPA with an error message
    private IActionResult RedirectToSpa(string returnUrl, string? error = null)
    {
        var spaUrl = _config["Spa:Url"] ?? "https://localhost:7236";
        var redirectUrl = $"{spaUrl}{returnUrl}";

        if (!string.IsNullOrEmpty(error))
        {
            redirectUrl += $"?error={Uri.EscapeDataString(error)}";
        }
        
        return Redirect(redirectUrl);
    }
    
    // Helper method to redirect to the SPA with a JWT token
    private IActionResult RedirectToSpaWithToken(string returnUrl, string token)
    {
        var spaUrl = _config["Spa:Url"] ?? "https://localhost:7236";
        var redirectUrl = $"{spaUrl}{returnUrl}?token={token}";
        return Redirect(redirectUrl);
    }
} 