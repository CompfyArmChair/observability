using Shared.Instrumentation;
using System.Diagnostics.Metrics;

namespace WarehouseApi.Instrumentation;

public class OtelMeters : IOtelMeter
{	
	private Counter<int> StockAddedCounter { get; }
	private Counter<int> StockDeletedCounter { get; }
	private Counter<int> ReservedStockAddedCounter { get; }
	private Counter<int> ReservedStockDeletedCounter { get; }
	private Counter<int> SoldStockDeletedCounter { get; }
	private Counter<int> SoldStockAddedCounter { get; }
	private ObservableGauge<int> TotalStockGauge { get; }
	private int _totalStock = 0;
	private ObservableGauge<int> TotalStockReservedGauge { get; }
	private int _totalStockReservedGauge = 0;
	private ObservableGauge<int> TotalStockSoldGauge { get; }
	private int _totalStockSoldGauge = 0;

	public string MeterName { get; }

	public OtelMeters(string meterName = "Shop")
	{
		var meter = new Meter(meterName);
		MeterName = meterName;

		StockAddedCounter = meter.CreateCounter<int>("domain.shop.stock.added", "Stock");
		StockDeletedCounter = meter.CreateCounter<int>("domain.shop.stock.deleted", "Stock");
		ReservedStockAddedCounter = meter.CreateCounter<int>("domain.shop.stock.reserved.added", "Stock");
		ReservedStockDeletedCounter = meter.CreateCounter<int>("domain.shop.stock.reserved.deleted", "Stock");
		SoldStockAddedCounter = meter.CreateCounter<int>("domain.shop.stock.sold.added", "Stock");
		SoldStockDeletedCounter = meter.CreateCounter<int>("domain.shop.stock.sold.deleted", "Stock");
		TotalStockGauge = meter.CreateObservableGauge<int>("domain.shop.stock.total", () => _totalStock);
		TotalStockReservedGauge = meter.CreateObservableGauge<int>("domain.shop.stock.reserved.total", () => _totalStockReservedGauge);
		TotalStockSoldGauge = meter.CreateObservableGauge<int>("domain.shop.stock.sold.total", () => _totalStockSoldGauge);
	}

	public void AddStock(int quantity) => StockAddedCounter.Add(quantity);
	public void DeleteStock() => StockDeletedCounter.Add(1);
	public void AddReserveStock(int quantity) => ReservedStockAddedCounter.Add(quantity);
	public void DeleteReservedStock() => ReservedStockDeletedCounter.Add(1);
	public void AddSoldStock(int quantity) => SoldStockAddedCounter.Add(quantity);
	public void DeleteSoldStock() => SoldStockDeletedCounter.Add(1);
	public void IncreaseTotalStock(int quantity) => _totalStock+= quantity;
	public void DecreaseTotalStock() => _totalStock--;
	public void IncreaseTotalReservedStock(int quantity) => _totalStockReservedGauge+=quantity;
	public void DecreaseTotalReservedStock() => _totalStockReservedGauge--;
	public void IncreaseTotalSoldStock() => _totalStockSoldGauge++;
	public void DecreaseTotalSoldStock() => _totalStockSoldGauge--;
}
