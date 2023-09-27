using StockManagementWebsite.Shared.StockItems;

namespace StockManagementWebsite.Client.Features.StockItems.Store.Actions;

public class StockItemAddedAction
{
    public StockItemDto AddedItem { get; }

    public StockItemAddedAction(StockItemDto addedItem)
    {
        AddedItem = addedItem;
    }
}
