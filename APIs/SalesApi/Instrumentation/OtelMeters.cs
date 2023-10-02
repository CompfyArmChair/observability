using Shared.Instrumentation.Metrics;
using System.Diagnostics.Metrics;

namespace SalesApi.Instrumentation;

public class OtelMeters : IOtelMeter
{	
	private Counter<int> PriceAddedCounter { get; }
	private Counter<int> PriceDeletedCounter { get; }
	private Counter<int> PriceUpdatedCounter { get; }	

	public string MeterName { get; }	

	public OtelMeters(string meterName = "Shop")
	{
		var meter = new Meter(meterName);
		MeterName = meterName;

		PriceAddedCounter = meter.CreateCounter<int>("domain.shop.price.added", "Price");
		PriceDeletedCounter = meter.CreateCounter<int>("domain.shop.price.deleted", "Price");
		PriceUpdatedCounter = meter.CreateCounter<int>("domain.shop.price.updated", "Price");		
	}

	public void AddPrice() => PriceAddedCounter.Add(1);
	public void DeletePrice() => PriceDeletedCounter.Add(1);
	public void UpdatePrice() => PriceUpdatedCounter.Add(1);
}
