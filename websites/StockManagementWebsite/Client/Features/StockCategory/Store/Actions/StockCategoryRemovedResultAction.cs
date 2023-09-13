using StockManagementWebsite.Shared;

namespace StockManagementWebsite.Client.Features.StockCategory.Store.Actions;

public class StockCategoryRemovedResultAction
{
	public int RemovedCategoryId { get; }

	public StockCategoryRemovedResultAction(int removedCategoryId)
	{
		RemovedCategoryId = removedCategoryId;
	}
}