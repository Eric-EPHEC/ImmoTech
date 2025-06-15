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

    // GET /auth/external/{provider}
    [HttpGet("{provider}")]
    public IActionResult ExternalLogin([FromRoute] string provider, [FromQuery] string? returnUrl)
    {
        returnUrl ??= "/external-callback"; // SPA route
        var redirectUrl = Url.Action("ExternalLoginCallback", "ExternalAuth", new { returnUrl });
        var props = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(props, provider);
    }

    // GET /auth/external/callback
    [HttpGet("callback")] // provider-specific handlers redirect here
    public async Task<IActionResult> ExternalLoginCallback([FromQuery] string returnUrl)
    {
        returnUrl = string.IsNullOrWhiteSpace(returnUrl) ? "/external-callback" : returnUrl;
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return Redirect($"{_config["Spa:Url"]}/login?error=external");
        }

        // try sign-in the user
        var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
        User user;
        if (!signInResult.Succeeded)
        {
            // if user does not exist, create one quickly with minimal fields
            var email = info.Principal.FindFirstValue(ClaimTypes.Email) ?? $"{info.ProviderKey}@placeholder.com";
            user = new User { UserName = email, Email = email };
            await _userManager.CreateAsync(user);
            await _userManager.AddLoginAsync(user, info);
        }
        else
        {
            user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        }

        // generate JWT for the signed-in user
        var token = _tokenGenerator.GenerateToken(user);

        // redirect back to SPA with token query param
        var spaUrl = _config["Spa:Url"] ?? "https://localhost:7236";
        return Redirect($"{spaUrl}{returnUrl}?token={token}");
    }
} 