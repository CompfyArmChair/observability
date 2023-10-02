using Shared.Instrumentation.Metrics;
using System.Diagnostics.Metrics;

namespace BasketApi.Instrumentation;

public class OtelMeters : IOtelMeter
{		
	private Counter<int> BasketCompletedCounter { get; }

	public string MeterName { get; }

	public OtelMeters(string meterName = "Shop")
	{
		var meter = new Meter(meterName);
		MeterName = meterName;

		BasketCompletedCounter = meter.CreateCounter<int>("domain.shop.basket.completed", "Basket");		
	}
	
	public void BasketCompleted() => BasketCompletedCounter.Add(1);
}
