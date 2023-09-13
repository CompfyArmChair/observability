using StockManagementWebsite.Shared;

namespace StockManagementWebsite.Client.Features.StockCategory.Store.Actions;

public class StockCategoryAddedResultAction
{
	public StockCategoryDto AddedCategory { get; }

	public StockCategoryAddedResultAction(StockCategoryDto addedCategory)
	{
		AddedCategory = addedCategory;
	}
}
