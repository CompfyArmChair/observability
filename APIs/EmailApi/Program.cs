using EmailApi.Instrumentation;
using MassTransit;
using Shared.Instrumentation;
using Shared.Instrumentation.MassTransit;
using Shared.Instrumentation.Metrics;
using Shared.ServiceBus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(config =>
	config.AddDefault("EmailApi", builder.Configuration.GetConnectionString("ServiceBus")!, (context, configurator) => configurator.UseSendAndPublishFilter(typeof(TelemetrySendFilter<>), context)));

builder.Services.AddOpenTelemetry(
	"EmailApi", 
	builder.Configuration.GetConnectionString("ApplicationInsights")!,
	new OtelMetricsConfiguration<OtelMeters>(new OtelMeters()));
//builder.Services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

app.Run();
