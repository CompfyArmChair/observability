using StockManagementWebsite.Shared.StockItems;

namespace StockManagementWebsite.Client.Features.StockItems.Store.Actions;

public class AddStockItemAction
{
    public AddStockItemsDto StockItemToAdd { get; }

    public AddStockItemAction(AddStockItemsDto stockItemsToAdd)
    {
        StockItemToAdd = stockItemsToAdd;
    }
}
