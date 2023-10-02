using Shared.Instrumentation.Metrics;
using System.Diagnostics.Metrics;

namespace ShippingApi.Instrumentation;

public class OtelMeters : IOtelMeter
{	
	private Counter<int> ShipmentAddedCounter { get; }
	private ObservableGauge<int> TotalShipmentsGauge { get; }
	private int _totalShipments = 0;

	private Counter<int> ShipmentCompletedCounter { get; }
	private ObservableGauge<int> TotalShipmentsCompletedGauge { get; }
	private int _totalShipmentsCompleted = 0;

	public string MeterName { get; }

	public OtelMeters(string meterName = "Shop")
	{
		var meter = new Meter(meterName);
		MeterName = meterName;

		ShipmentAddedCounter = meter.CreateCounter<int>("domain.shop.shipment.added", "Shipment");
		TotalShipmentsGauge = meter.CreateObservableGauge<int>("domain.shop.shipment.total", () => _totalShipments);

		ShipmentCompletedCounter = meter.CreateCounter<int>("domain.shop.shipment.completed", "Shipment");
		TotalShipmentsCompletedGauge = meter.CreateObservableGauge<int>("domain.shop.shipment.completed.total", () => _totalShipmentsCompleted);
	}

	public void AddShipment() => ShipmentAddedCounter.Add(1);
	public void IncreaseTotalShipments() => _totalShipments++;

	public void CompleteShipment() => ShipmentCompletedCounter.Add(1);
	public void IncreaseTotalShipmentsCompleted() => _totalShipmentsCompleted++;

}
