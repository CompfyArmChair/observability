using StockManagementWebsite.Shared.StockItems;

namespace StockManagementWebsite.Client.Features.StockItems.Store.Actions;

public class FetchStockItemResultAction
{
    public IEnumerable<StockItemDto> StockItems { get; }

    public FetchStockItemResultAction(IEnumerable<StockItemDto> stockItems)
    {
        StockItems = stockItems;
    }
}
