using Shared.Instrumentation.Metrics;
using System.Diagnostics.Metrics;

namespace CatalogueApi.Instrumentation;

public class OtelMeters : IOtelMeter
{		
	private Counter<int> CategoriesAddedCounter { get; }
	private Counter<int> CategoriesDeletedCounter { get; }
	private Counter<int> CategoriesUpdatedCounter { get; }
	private ObservableGauge<int> TotalCategoriesGauge { get; }
	private int _totalCategories = 0;

	public string MeterName { get; }

	public OtelMeters(string meterName = "Shop")
	{
		var meter = new Meter(meterName);
		MeterName = meterName;

		CategoriesAddedCounter = meter.CreateCounter<int>("domain.shop.categories.added", "Category");
		CategoriesDeletedCounter = meter.CreateCounter<int>("domain.shop.categories.deleted", "Category");
		CategoriesUpdatedCounter = meter.CreateCounter<int>("domain.shop.categories.updated", "Category");
		TotalCategoriesGauge = meter.CreateObservableGauge<int>("domain.shop.categories.total", () => _totalCategories);
	}
	
	public void AddCategory() => CategoriesAddedCounter.Add(1);
	public void DeleteCategory() => CategoriesDeletedCounter.Add(1);
	public void UpdateCategory() => CategoriesUpdatedCounter.Add(1);
	public void IncreaseTotalCategories() => _totalCategories++;
	public void DecreaseTotalCategories() => _totalCategories--;
}
