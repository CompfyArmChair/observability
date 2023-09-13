namespace StockManagementWebsite.Client.Features.StockItems.Store.Actions;

public class FetchStockItemsForSkuAction
{
    public string Sku { get; }

    public FetchStockItemsForSkuAction(string sku)
    {
        Sku = sku;
    }
}
