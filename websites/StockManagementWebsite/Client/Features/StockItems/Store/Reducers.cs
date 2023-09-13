using Fluxor;
using StockManagementWebsite.Client.Features.StockItems.Store.Actions;
using StockManagementWebsite.Shared;

namespace StockManagementWebsite.Client.Features.StockItems.Store;

public class Reducers
{
    // This reducer handles the start of fetching the stock items
    [ReducerMethod(typeof(FetchStockItemsAction))]
    public static StockItemsState ReduceFetchStockItemsAction(StockItemsState _)
        => new(isLoading: true, stockItems: Enumerable.Empty<StockItemDto>());

    // This reducer handles the result of fetching the stock items
    [ReducerMethod]
    public static StockItemsState ReduceFetchStockItemsResultAction(StockItemsState _, FetchStockItemsResultAction action)
        => new(isLoading: false, stockItems: action.StockItems);

    // This reducer handles the addition of a new stock item
    [ReducerMethod]
    public static StockItemsState ReduceStockItemAddedSuccessfullyAction(StockItemsState state, StockItemAddedSuccessfullyAction action)
    {
        var updatedStockItems = state.StockItems.ToList();
        updatedStockItems.Add(action.AddedItem);

        return new StockItemsState(isLoading: state.IsLoading, stockItems: updatedStockItems);
    }
}
