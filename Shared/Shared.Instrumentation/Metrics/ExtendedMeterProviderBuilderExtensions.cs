using OpenTelemetry.Metrics;

namespace Shared.Instrumentation.Metrics;
public static class MeterProviderBuilderExtensions
{	
	public static MeterProviderBuilder AddExtendedProcessInstrumentation(
		this MeterProviderBuilder builder)
	{
		builder.AddMeter(ExtendedProcessMetrics.MeterName);
		return builder.AddInstrumentation(() => new ExtendedProcessMetrics());
	}

	public static MeterProviderBuilder AddExtendedSystemInstrumentation(
	this MeterProviderBuilder builder)
	{
		builder.AddMeter(ExtendedSystemMetrics.MeterName);
		return builder.AddInstrumentation(() => new ExtendedSystemMetrics());
	}
}