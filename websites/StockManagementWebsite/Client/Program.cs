using Fluxor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Blazor.WebAssembly.Telemetry;
using System.Runtime.Versioning;
using StockManagementWebsite.Client;

[assembly: SupportedOSPlatform("browser")]


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

builder.Services.AddBlazorWasmTelemetry(builder.Configuration);

var currentAssembly = typeof(Program).Assembly;
builder.Services.AddFluxor(options =>
{
    options.ScanAssemblies(currentAssembly);
    options.UseReduxDevTools();
});

await builder.Build().RunAsync();
