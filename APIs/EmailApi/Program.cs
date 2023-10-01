using MassTransit;
using Shared.ServiceBus;
using Shared.Instrumentation;
using Microsoft.ApplicationInsights.Extensibility;
using EmailApi;
using Shared.Instrumentation.MassTransit;
using EmailApi.Instrumentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(config =>
	config.AddDefault("EmailApi", builder.Configuration.GetConnectionString("ServiceBus")!, (context, configurator) => configurator.UseSendAndPublishFilter(typeof(TelemetrySendFilter<>), context)));

builder.Services.AddOpenTelemetry(
	"EmailApi", 
	builder.Configuration.GetConnectionString("ApplicationInsights")!,
	new OtelMetricsConfiguration<OtelMeters>(new OtelMeters()));
//builder.Services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();

var app = builder.Build();

app.Run();
