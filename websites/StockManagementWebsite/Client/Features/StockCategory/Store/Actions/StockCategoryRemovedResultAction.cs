namespace StockManagementWebsite.Client.Features.StockCategory.Store.Actions;

public class StockCategoryRemovedResultAction
{
	public string Sku { get; }

	public StockCategoryRemovedResultAction(string sku)
	{
		Sku = sku;
	}
}