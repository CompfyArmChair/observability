namespace Shared.Instrumentation.Metrics;

public interface IOtelMetricsConfiguration<out T> where T : IOtelMeter
{
    T Meters { get; }
    OtelMeterView[] MeterViews { get; }
}