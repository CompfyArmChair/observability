using Fluxor;
using StockManagementWebsite.Shared;
using System.Net.Http;

namespace StockManagementWebsite.Client.Features.StockItems.Store.Actions;

public class AddStockItemAction
{
    public StockItemDto StockItemToAdd { get; }

    public AddStockItemAction(StockItemDto stockItemToAdd)
    {
        StockItemToAdd = stockItemToAdd;
    }
}




