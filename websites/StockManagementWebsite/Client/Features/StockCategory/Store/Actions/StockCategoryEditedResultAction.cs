using StockManagementWebsite.Shared.StockCategories;

namespace StockManagementWebsite.Client.Features.StockCategory.Store.Actions;

public class StockCategoryEditedResultAction
{
	public StockCategoryDto EditedCategory { get; }

	public StockCategoryEditedResultAction(StockCategoryDto editedCategory)
	{
		EditedCategory = editedCategory;
	}
}
