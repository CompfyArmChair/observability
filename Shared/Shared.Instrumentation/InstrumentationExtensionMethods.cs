using Azure.Monitor.OpenTelemetry.Exporter;
using MassTransit.Logging;
using MassTransit.Monitoring;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Shared.Instrumentation.Metrics;
using System.Diagnostics;

namespace Shared.Instrumentation;

public static class InstrumentationExtensionMethods
{
	private static readonly ActivityListener BaggagePropagationActivityListener = new()
	{
		ShouldListenTo = _ => true,
		ActivityStopped = activity =>
		{
			foreach (var (key, value) in activity.Baggage)
			{
				activity.AddTag(key, value);
			}
		}
	};

	public static OpenTelemetryBuilder AddOpenTelemetry(this IServiceCollection services, string serviceName, string connectionString, IOtelMetricsConfiguration<IOtelMeter> metricsConfiguration = null)
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

		//ActivitySource.AddActivityListener(BaggagePropagationActivityListener);

		return services.AddOpenTelemetry()
			.ConfigureResource(resourceBuilder =>
				ConfigureResource(resourceBuilder, serviceName))
			.WithTracing(tracerProviderBuilder =>
				tracerProviderBuilder
					.AddSource(DiagnosticHeaders.DefaultListenerName)
					.AddAspNetCoreInstrumentation()
					//.AddHttpClientInstrumentation(options => options.RecordException = true)
					.AddEntityFrameworkCoreInstrumentation()
					.AddAzureMonitorTraceExporter(exporter =>
					{
						exporter.ConnectionString = connectionString;
					}))
			.WithMetrics(meterProviderBuilder =>
			{
				meterProviderBuilder
					.AddMeter(InstrumentationOptions.MeterName)
					.AddAspNetCoreInstrumentation() //Incoming
					.AddHttpClientInstrumentation() //Outgoing
					.AddRuntimeInstrumentation() //Exceptions count, number of thread pools, etc
					.AddProcessInstrumentation() //CPU, memory etc
					.AddExtendedProcessInstrumentation() //User and system process CPU utilisation
					.AddExtendedSystemInstrumentation() //System CPU utilisation
					.AddAzureMonitorMetricExporter(o =>
					{
						o.ConnectionString = connectionString;
					});

				if(metricsConfiguration is not null)
				{
					services.AddSingleton(metricsConfiguration.Meters.GetType(), metricsConfiguration.Meters);
					meterProviderBuilder.AddMeter(metricsConfiguration.Meters.MeterName);
					foreach (var view in metricsConfiguration.MeterViews)
					{
						meterProviderBuilder.AddView(view.InstrumentName, view.Configuration);
					}
				}
			});

		///It's fine to export directly to an agent/collector initially, but it's recommended to use an otlp exporter
		///.AddOtlpExporter(opts =>
		///{
		///			opts.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"]);
		///}));
	}

	public static OpenTelemetryBuilder WithBaggage<TBaggageSource>(this OpenTelemetryBuilder builder, string propertyName, Func<TBaggageSource, string> propertyCollector)
	{
		TelemetryBaggageHandler.Register(propertyName, propertyCollector);
		return builder;
	}

	public static void AddInsightsTelemetry(this IServiceCollection services, string connectionString)
	{
		services.AddApplicationInsightsTelemetry(options =>
			{
				options.ConnectionString = connectionString;
			});
	}
}