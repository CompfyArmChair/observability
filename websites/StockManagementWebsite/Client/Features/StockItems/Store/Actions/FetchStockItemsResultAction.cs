using StockManagementWebsite.Shared;

namespace StockManagementWebsite.Client.Features.StockItems.Store.Actions;

public class FetchStockItemsResultAction
{
    public IEnumerable<StockItemDto> StockItems { get; }

    public FetchStockItemsResultAction(IEnumerable<StockItemDto> stockItems)
    {
        StockItems = stockItems;
    }
}
