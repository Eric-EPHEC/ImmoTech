using Immotech.Front;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.Toast;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7261") });
builder.Services.AddBlazoredToast();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, Immotech.Front.Services.TokenAuthenticationStateProvider>();
await builder.Build().RunAsync();
