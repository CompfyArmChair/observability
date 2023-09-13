using Fluxor;
using StockManagementWebsite.Shared;

namespace StockManagementWebsite.Client.Features.StockItems.Store;

[FeatureState]
public class StockItemsState
{
    public bool IsLoading { get; }
    public IEnumerable<StockItemDto> StockItems { get; }

    // Default constructor
    private StockItemsState()
    {
        StockItems = Enumerable.Empty<StockItemDto>();
    }

    // Constructor that sets the actual properties
    public StockItemsState(bool isLoading, IEnumerable<StockItemDto> stockItems)
    {
        IsLoading = isLoading;
        StockItems = stockItems ?? Enumerable.Empty<StockItemDto>();
    }
}
