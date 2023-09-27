namespace StockManagementWebsite.Client.Features.StockCategory.Store.Actions;

public class RemoveStockCategoryAction
{
    public string Sku { get; }

    public RemoveStockCategoryAction(string sku)
    {
        Sku = sku;
    }
}
