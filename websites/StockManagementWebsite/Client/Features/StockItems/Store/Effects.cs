using Fluxor;
using StockManagementWebsite.Client.Features.StockItems.Store.Actions;
using StockManagementWebsite.Shared.StockItems;
using System.Net.Http.Json;

namespace StockManagementWebsite.Client.Features.StockItems.Store;

public class Effects
{
    private readonly HttpClient _httpClient;

    public Effects(HttpClient client) => _httpClient = client;

    [EffectMethod]
    public async Task HandleFetchStockItemForSkuAction(FetchStockItemForSkuAction action, IDispatcher dispatcher)
    {
        var stockItems = await _httpClient.GetFromJsonAsync<StockItemDto[]>($"api/StockItems/{action.Sku}")
            ?? Array.Empty<StockItemDto>();

        dispatcher.Dispatch(new FetchStockItemResultAction(stockItems));
    }

    [EffectMethod]
    public async Task HandleAddStockItemAction(AddStockItemAction action, IDispatcher dispatcher)
    {
        var response = await _httpClient.PostAsJsonAsync("api/StockItems", action.StockItemToAdd);
        
        //var addedItem = await response.Content.ReadFromJsonAsync<StockItemDto>();
        //dispatcher.Dispatch(new StockItemAddedAction(addedItem));

    }

    [EffectMethod]
    public async Task HandleDeleteStockItemAction(DeleteStockItemAction action, IDispatcher dispatcher)
    {
        await _httpClient.DeleteAsync($"api/StockItems/{action.Id}");
        
        dispatcher.Dispatch(new StockItemDeletedResultAction(action.Id));
    }
}
