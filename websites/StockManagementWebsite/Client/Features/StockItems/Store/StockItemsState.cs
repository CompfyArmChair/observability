using Fluxor;
using StockManagementWebsite.Shared.StockItems;

namespace StockManagementWebsite.Client.Features.StockItems.Store;

[FeatureState]
public class StockItemsState
{
    public bool IsLoading { get; }
    public IEnumerable<StockItemDto> StockItems { get; }

    private StockItemsState()
    {
        StockItems = Enumerable.Empty<StockItemDto>();
    }

    public StockItemsState(bool isLoading, IEnumerable<StockItemDto> stockItems)
    {
        IsLoading = isLoading;
        StockItems = stockItems ?? Enumerable.Empty<StockItemDto>();
    }
}
