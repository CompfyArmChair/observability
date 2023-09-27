using Fluxor;
using StockManagementWebsite.Client.Features.StockItems.Store.Actions;
using StockManagementWebsite.Shared.StockItems;

namespace StockManagementWebsite.Client.Features.StockItems.Store;

public class Reducers
{
    [ReducerMethod(typeof(FetchStockItemsAction))]
    public static StockItemsState ReduceFetchStockItemsAction(StockItemsState _)
        => new(isLoading: true, stockItems: Enumerable.Empty<StockItemDto>());
    
    [ReducerMethod]
    public static StockItemsState ReduceFetchStockItemsResultAction(StockItemsState _, FetchStockItemResultAction action)
        => new(isLoading: false, stockItems: action.StockItems);
    
    [ReducerMethod]
    public static StockItemsState ReduceStockItemAddedSuccessfullyAction(StockItemsState state, StockItemAddedAction action)
    {
        var updatedStockItems = state.StockItems.ToList();
        updatedStockItems.Add(action.AddedItem);

        return new StockItemsState(isLoading: state.IsLoading, stockItems: updatedStockItems);
    }

    [ReducerMethod]
    public static StockItemsState ReduceStockItemDeletedResultAction(
        StockItemsState state,
        StockItemDeletedResultAction action)
    {
        var newList = state.StockItems
            .Where(x => x.Id != action.Id)
            .ToList();

        return new StockItemsState(false, newList);
    }
}
