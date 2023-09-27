using MassTransit;
using Shared.Instrumentation;
using Shared.ServiceBus;
using StockManagementWebsite.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddHttpClient();

builder.Services.AddSignalR();

builder.Services.AddMassTransit(config =>
    config.AddDefault("StockManagementWebsite.Server", builder.Configuration.GetConnectionString("ServiceBus")!));

builder.Services.AddOpenTelemetry("StockManagementBFF", builder.Configuration.GetConnectionString("ApplicationInsights")!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error");
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapHub<StockManagementHub>("/StockManagementHub");
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
