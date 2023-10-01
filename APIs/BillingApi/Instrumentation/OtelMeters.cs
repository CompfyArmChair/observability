using Shared.Instrumentation;
using System.Diagnostics.Metrics;

namespace BillingApi.Instrumentation;

public class OtelMeters : IOtelMeter
{		
	private Counter<int> BillingAddedCounter { get; }
	private ObservableGauge<int> TotalBillingsGauge { get; }
	private int _totalBillings = 0;

	private Counter<int> BillingCompletedCounter { get; }
	private ObservableGauge<int> TotalBillingsCompletedGauge { get; }
	private int _totalBillingsCompleted = 0;

	public string MeterName { get; }

	public OtelMeters(string meterName = "Shop")
	{
		var meter = new Meter(meterName);
		MeterName = meterName;

		BillingAddedCounter = meter.CreateCounter<int>("domain.shop.billing.added", "Billing");
		TotalBillingsGauge = meter.CreateObservableGauge<int>("domain.shop.billing.total", () => _totalBillings);

		BillingCompletedCounter = meter.CreateCounter<int>("domain.shop.billing.completed", "Billing");
		TotalBillingsCompletedGauge = meter.CreateObservableGauge<int>("domain.shop.billings.completed.total", () => _totalBillingsCompleted);
	}
	
	public void AddBilling() => BillingAddedCounter.Add(1);
	public void IncreaseTotalBillings() => _totalBillings++;
	public void CompleteBilling() => BillingCompletedCounter.Add(1);	
	public void IncreaseTotalBillingsCompleted() => _totalBillingsCompleted++;

}
