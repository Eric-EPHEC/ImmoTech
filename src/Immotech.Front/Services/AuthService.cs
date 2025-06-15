using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Immotech.Front.Services;

// This service manages the authentication state of the user.
public class AuthService
{
    private readonly IJSRuntime _js;
    private readonly AuthenticationStateProvider _authStateProvider;

    // We inject the JS runtime to interact with localStorage and the auth state provider to notify it of changes.
    public AuthService(IJSRuntime js, AuthenticationStateProvider authStateProvider)
    {
        _js = js;
        _authStateProvider = authStateProvider;
    }

    // This method checks if the user is authenticated by looking for a token in local storage.
    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await _js.InvokeAsync<string?>("localStorage.getItem", "authToken");
        return !string.IsNullOrWhiteSpace(token);
    }
    
    // This method is called after a user logs in successfully.
    public async Task OnLoginAsync(string token)
    {
        // We store the token in local storage.
        await _js.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        // We notify the auth state provider that the user is now authenticated.
        ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(token);
    }

    // This method is called to log the user out.
    public async Task OnLogoutAsync()
    {
        // We remove the token from local storage.
        await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
        // We notify the auth state provider that the user has logged out.
        ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
    }
} 