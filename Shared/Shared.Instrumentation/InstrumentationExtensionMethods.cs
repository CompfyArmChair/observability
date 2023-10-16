using Azure.Monitor.OpenTelemetry.AspNetCore;
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
			foreach (var (key, value) in activity.Baggage.Where(x => x.Value != null))
			{
				if (activity.Tags.All(x => x.Key != key))
				{
					activity.AddTag(key, value);
				}
			}
		}
	};

	public static OpenTelemetryBuilder AddOpenTelemetry(this IServiceCollection services, string serviceName, string connectionString, IOtelMetricsConfiguration<IOtelMeter> metricsConfiguration = null)
	{
		//Uncomment this line to see Experimental telemetry.
		//AppContext.SetSwitch("Azure.Experimental.EnableActivitySource", true);

		services.AddLogging(builder =>
		{
			builder.ClearProviders();
			builder.SetMinimumLevel(LogLevel.Trace);
			builder.AddOpenTelemetry();
		});

		ActivitySource.AddActivityListener(BaggagePropagationActivityListener);

		var Otelbuilder = services.AddOpenTelemetry()
			.UseAzureMonitor(o => o.ConnectionString = connectionString);

		services.ConfigureOpenTelemetryTracerProvider((sp, builder) => builder
			.AddSource(DiagnosticHeaders.DefaultListenerName)
			.ConfigureResource(resourceBuilder => resourceBuilder.AddService(serviceName)));

		if (metricsConfiguration is not null)
		{
			services.AddSingleton(metricsConfiguration.Meters.GetType(), metricsConfiguration.Meters);
		}
		
		services.ConfigureOpenTelemetryMeterProvider((sp, builder) =>
		{
			builder
				.AddMeter(InstrumentationOptions.MeterName)
				.AddExtendedProcessInstrumentation()
				.AddExtendedSystemInstrumentation();

			if (metricsConfiguration is not null)
			{				
				builder.AddMeter(metricsConfiguration.Meters.MeterName);
				foreach (var view in metricsConfiguration.MeterViews)
				{
					builder.AddView(view.InstrumentName, view.Configuration);
				}
			}
		});
		return Otelbuilder;


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

	//public static void AddInsightsTelemetry(this IServiceCollection services, string connectionString)
	//{
	//	services.AddApplicationInsightsTelemetry(options =>
	//		{
	//			options.ConnectionString = connectionString;
	//		});
	//}
}