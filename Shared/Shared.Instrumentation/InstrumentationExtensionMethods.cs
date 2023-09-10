using Azure.Monitor.OpenTelemetry.Exporter;
using MassTransit.Logging;
using MassTransit.Monitoring;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Shared.Instrumentation;

public static class InstrumentationExtensionMethods
{
	public static void AddOpenTelemetry(this IServiceCollection services, string serviceName, string connectionString)
	{
		services.AddLogging(builder =>
		{
			builder.ClearProviders();
			builder.SetMinimumLevel(LogLevel.Trace);
			builder.AddOpenTelemetry();
		});


		static void ConfigureResource(ResourceBuilder builder, string serviceName)
		{
			builder
				.AddService(serviceName)
				.AddTelemetrySdk()
				.AddEnvironmentVariableDetector();
		}

		services.AddOpenTelemetry()
			.ConfigureResource(resourceBuilder =>
				ConfigureResource(resourceBuilder, serviceName))			
			.WithTracing(tracerProviderBuilder =>
				tracerProviderBuilder
					.AddSource(DiagnosticHeaders.DefaultListenerName)
					.AddAspNetCoreInstrumentation()
					.AddEntityFrameworkCoreInstrumentation()					
					.AddAzureMonitorTraceExporter(exporter =>
					{
						exporter.ConnectionString = connectionString;
					}))
			.WithMetrics(meterProviderBuilder =>
				meterProviderBuilder
					.AddMeter(InstrumentationOptions.MeterName)
					.AddAspNetCoreInstrumentation() //Incoming
					.AddHttpClientInstrumentation() //Outgoing
					.AddRuntimeInstrumentation() //Exceptions count, number of thread pools, etc
					.AddProcessInstrumentation() //CPU, memory etc					
					.AddAzureMonitorMetricExporter(o =>
					{
						o.ConnectionString = connectionString;
					}));
					///It's fine to export directly to an agent/collector initially, but it's recommended to use an otlp exporter
					///.AddOtlpExporter(opts =>
					///{
					///			opts.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"]);
					///}));
	}
	public static void AddInsightsTelemetry(this IServiceCollection services, string connectionString)
	{
		services.AddApplicationInsightsTelemetry(options =>
			{
				options.ConnectionString = connectionString;
			});
	}
}