using Fluxor;
using StockManagementWebsite.Client.Features.StockItems.Store.Actions;
using StockManagementWebsite.Shared;
using System.Net.Http.Json;

namespace StockManagementWebsite.Client.Features.StockItems.Store;

public class Effects
{
    private readonly HttpClient _httpClient;

    public Effects(HttpClient client) => _httpClient = client;

    [EffectMethod(typeof(FetchStockItemsForSkuAction))]
    public async Task HandleFetchStockItemsForSkuAction(FetchStockItemsForSkuAction action, IDispatcher dispatcher)
    {
        var stockItems = await _httpClient.GetFromJsonAsync<IEnumerable<StockItemDto>>($"api/StockItems/{action.Sku}")
            ?? Enumerable.Empty<StockItemDto>();

        dispatcher.Dispatch(new FetchStockItemsResultAction(stockItems));
    }

    [EffectMethod(typeof(AddStockItemAction))]
    public async Task HandleAddStockItemAction(AddStockItemAction action, IDispatcher dispatcher)
    {
        var response = await _httpClient.PostAsJsonAsync("api/StockItems", action.StockItemToAdd);

        if (response.IsSuccessStatusCode)
        {
            // Handle success - for example, dispatch another action to fetch the updated list of stock items.
            var addedItem = await response.Content.ReadFromJsonAsync<StockItemDto>();
            dispatcher.Dispatch(new StockItemAddedSuccessfullyAction(addedItem));
        }
        else
        {
            // Handle failure - for example, show an error message or dispatch a failure action.
        }
    }

    [EffectMethod(typeof(FetchStockItemsAction))]
    public async Task HandleFetchStockItemsAction(IDispatcher dispatcher)
    {
        var stockItems = await _httpClient.GetFromJsonAsync<IEnumerable<StockItemDto>>("api/StockItems")
            ?? Enumerable.Empty<StockItemDto>();

        dispatcher.Dispatch(new FetchStockItemsResultAction(stockItems));
    }
}
