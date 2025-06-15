using Immotech.Front;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.Toast;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7261") });
builder.Services.AddBlazoredToast();
builder.Services.AddScoped<Immotech.Front.Services.AuthService>();

await builder.Build().RunAsync();
