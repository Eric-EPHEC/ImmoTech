using Microsoft.JSInterop;

namespace Immotech.Front.Services;

// Simple client-side auth helper. Checks if a token is stored in localStorage.
public class AuthService
{
    private readonly IJSRuntime _js;

    // inject JS runtime so we can talk to localStorage
    public AuthService(IJSRuntime js)
    {
        _js = js;
    }

    // quick check used by pages to know if the user is logged in
    public async Task<bool> IsAuthenticatedAsync()
    {
        // read token from localStorage
        var token = await _js.InvokeAsync<string?>("localStorage.getItem", "authToken");
        // authenticated if we have any non-empty token
        return !string.IsNullOrWhiteSpace(token);
    }

    // clear token from storage -> user becomes anonymous
    public async Task LogoutAsync()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
    }
} 