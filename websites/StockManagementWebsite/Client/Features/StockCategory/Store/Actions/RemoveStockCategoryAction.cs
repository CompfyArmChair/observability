namespace StockManagementWebsite.Client.Features.StockCategory.Store.Actions;

public class RemoveStockCategoryAction
{
    public int CategoryId { get; }

    public RemoveStockCategoryAction(int categoryId)
    {
        CategoryId = categoryId;
    }
}
