namespace Shared.Instrumentation.Metrics;

public class OtelMetricsConfiguration<T> : IOtelMetricsConfiguration<T> where T : IOtelMeter
{
    public T Meters { get; }

    public OtelMeterView[] MeterViews { get; }

    public OtelMetricsConfiguration(T meters, OtelMeterView[]? meterViews = null)
    {
        Meters = meters;
        MeterViews = meterViews ?? Array.Empty<OtelMeterView>();
    }
}