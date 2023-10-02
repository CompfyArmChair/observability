using OpenTelemetry.Metrics;
using Shared.Instrumentation.Metrics;
using System.Diagnostics.Metrics;

namespace OrderApi.Instrumentation;

public class OtelMeters : IOtelMeter
{	
	private Counter<int> OrderAddedCounter { get; }		
	private ObservableGauge<int> TotalOrdersGauge { get; }
	private int _totalOrders = 0;

	private Counter<int> OrderShippedCounter { get; }
	private ObservableGauge<int> TotalOrdersShippedGauge { get; }
	private int _totalOrdersShipped = 0;

	private Counter<int> OrderBilledCounter { get; }
	private ObservableGauge<int> TotalOrdersBilledGauge { get; }
	private int _totalOrdersBilled = 0;

	private Histogram<int> NumberOfProductsPerOrderHistogram { get; }

	public string MeterName { get; }	

	public OtelMeterView[] MeterViews { get; }

	public OtelMeters(string meterName = "Shop")
	{
		var meter = new Meter(meterName);
		MeterName = meterName;

		OrderAddedCounter = meter.CreateCounter<int>("domain.shop.order.added", "order");		
		TotalOrdersGauge = meter.CreateObservableGauge<int>("domain.shop.order.total", () => _totalOrders);

		OrderShippedCounter = meter.CreateCounter<int>("domain.shop.order.shipped", "order");
		TotalOrdersGauge = meter.CreateObservableGauge<int>("domain.shop.order.shipped.total", () => _totalOrdersShipped);

		OrderBilledCounter = meter.CreateCounter<int>("domain.shop.order.billed", "order");
		TotalOrdersBilledGauge = meter.CreateObservableGauge<int>("domain.shop.order.billed.total", () => _totalOrdersBilled);

		NumberOfProductsPerOrderHistogram = meter.CreateHistogram<int>("domain.shop.order.number_of_products", "Products", "Number of products per order");
		MeterViews = new[]
		{
			new OtelMeterView()
			{
				InstrumentName = "orders-number-of-products",
				Configuration = new ExplicitBucketHistogramConfiguration { Boundaries = new double[] { 1, 2, 5, 7, 11 } }
			}
		};
	}

	public void AddOrder() => OrderAddedCounter.Add(1);		
	public void IncreaseOrders() => _totalOrders++;

	public void ShippedOrder() => OrderShippedCounter.Add(1);
	public void IncreaseShippedOrders() => _totalOrdersShipped++;

	public void BillOrder() => OrderBilledCounter.Add(1);
	public void IncreaseBilledOrders() => _totalOrdersBilled++;

	public void RecordNumberOfProducts(int amount) => NumberOfProductsPerOrderHistogram.Record(amount);
}
