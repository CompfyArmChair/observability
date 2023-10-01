using OrderApi.Instrumentation;

namespace Shared.Instrumentation;

public interface IOtelMetricsConfiguration<out T> where T : IOtelMeter
{
	T Meters { get; }
	OtelMeterView[] MeterViews { get; }
}