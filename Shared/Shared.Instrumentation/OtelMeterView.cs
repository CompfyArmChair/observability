using OpenTelemetry.Metrics;

namespace OrderApi.Instrumentation;

public class OtelMeterView
{
	public string InstrumentName { get; set; }

	public ExplicitBucketHistogramConfiguration Configuration { get; set; }
}