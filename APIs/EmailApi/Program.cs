using MassTransit;
using Shared.ServiceBus;
using Shared.Instrumentation;
using Microsoft.ApplicationInsights.Extensibility;
using EmailApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(config =>
	config.AddDefault("EmailApi"));

builder.Services.AddInstrumentation();
builder.Services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();

var app = builder.Build();

app.Run();
