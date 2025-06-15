using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.JSInterop;

namespace Immotech.Front.Services
{
    // This service provides authentication state to the application.
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        // This method is called by the framework to get the current authentication state.
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // We try to get the token from local storage.
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

                if (string.IsNullOrWhiteSpace(token))
                {
                    // If there's no token, the user is anonymous.
                    return new AuthenticationState(_anonymous);
                }

                // If there is a token, we parse it to create a claims principal.
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
                return new AuthenticationState(claimsPrincipal);
            }
            catch
            {
                // If anything goes wrong, we assume the user is anonymous.
                return new AuthenticationState(_anonymous);
            }
        }

        // This method notifies the application that the authentication state has changed.
        public void NotifyUserAuthentication(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        // This method notifies the application that the user has logged out.
        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(new AuthenticationState(_anonymous));
            NotifyAuthenticationStateChanged(authState);
        }

        // This helper method parses the claims from a JWT.
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            return token.Claims;
        }
    }
} 