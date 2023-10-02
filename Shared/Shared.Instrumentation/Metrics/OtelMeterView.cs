using OpenTelemetry.Metrics;

namespace Shared.Instrumentation.Metrics;

public class OtelMeterView
{
    public string InstrumentName { get; set; }

    public ExplicitBucketHistogramConfiguration Configuration { get; set; }
}