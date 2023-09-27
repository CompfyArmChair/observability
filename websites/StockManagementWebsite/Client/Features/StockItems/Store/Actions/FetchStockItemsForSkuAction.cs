namespace StockManagementWebsite.Client.Features.StockItems.Store.Actions;

public class FetchStockItemForSkuAction
{
    public string Sku { get; }

    public FetchStockItemForSkuAction(string sku)
    {
        Sku = sku;
    }
}
