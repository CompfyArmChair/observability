using StockManagementWebsite.Shared;

namespace StockManagementWebsite.Client.Features.StockItems.Store.Actions;

public class StockItemAddedSuccessfullyAction
{
    public StockItemDto AddedItem { get; }

    public StockItemAddedSuccessfullyAction(StockItemDto addedItem)
    {
        AddedItem = addedItem;
    }
}
