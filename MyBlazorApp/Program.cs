using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyBlazorApp;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
// Application services
builder.Services.AddSingleton<MyBlazorApp.Services.RegistrationService>();
builder.Services.AddScoped<MyBlazorApp.Services.UserSessionService>(sp => new MyBlazorApp.Services.UserSessionService(sp.GetRequiredService<IJSRuntime>()));

await builder.Build().RunAsync();
