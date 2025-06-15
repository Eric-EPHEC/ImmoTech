using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Immotech.Front.Services;

// This service manages the authentication state of the user.
public class AuthService
{
    private readonly IJSRuntime _js;
    private readonly TokenAuthenticationStateProvider _authStateProvider;

    // We inject the JS runtime to interact with localStorage and the auth state provider to notify it of changes.
    public AuthService(IJSRuntime js, TokenAuthenticationStateProvider authStateProvider)
    {
        _js = js;
        _authStateProvider = authStateProvider;
    }

    // This method checks if the user is authenticated by looking for a token in local storage.
    public async Task<bool> IsAuthenticatedAsync()
    {
        return _authStateProvider.GetAuthenticationStateAsync()
            .ContinueWith(t => t.Result.User.Identity?.IsAuthenticated ?? false);
    }
    
    // This method is called after a user logs in successfully.
    public async Task OnLoginAsync(string token)
    {
        
        // We notify the auth state provider that the user is now authenticated.
        ((CookieAuthenticationStateProvider)_authStateProvider).NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    // This method is called to log the user out.
    public async Task OnLogoutAsync()
    {
        // We notify the auth state provider that the user has logged out.
        ((CookieAuthenticationStateProvider)_authStateProvider).NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
} 

public class CookieAuthenticationStateProvider : TokenAuthenticationStateProvider, IDisposable
{
    public CookieAuthenticationStateProvider(
        IJSRuntime js,
        NavigationManager nav,
        IHttpClientFactory clientFactory,
        ILogger<CookieAuthenticationStateProvider> logger)
    {
        // sample code unchanged
    }

    // rest of the sample class…
}