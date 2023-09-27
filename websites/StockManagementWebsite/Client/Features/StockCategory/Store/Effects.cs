using Fluxor;
using ShopWebsite.Client.Features.Products.Store.Actions;
using StockManagementWebsite.Client.Features.StockCategory.Store.Actions;
using StockManagementWebsite.Shared.StockCategories;
using System.Net.Http.Json;

namespace StockManagementWebsite.Client.Features.StockCategory.Store;

public class Effects
{
    private readonly HttpClient _httpClient;

    public Effects(HttpClient client)
    {
        _httpClient = client;
    }

	[EffectMethod(typeof(FetchStockCategoriesAction))]
	public async Task HandleFetchProductsAction(IDispatcher dispatcher)
	{
		var categories = await _httpClient.GetFromJsonAsync<StockCategoryDto[]>("api/StockCategories")
			 ?? Enumerable.Empty<StockCategoryDto>();

		dispatcher.Dispatch(new FetchStockCategoriesResultAction(categories));
	}

	[EffectMethod]
    public async Task HandleAddStockCategoryAction(AddStockCategoryAction action, IDispatcher dispatcher)
    {
        var addedCategory = await _httpClient.PostAsJsonAsync("api/StockCategories", action.Category);

		// Assuming your API returns the created category
		//dispatcher.Dispatch(new StockCategoryAddedResultAction(addedCategory));
    }

    [EffectMethod]
    public async Task HandleEditStockCategoryAction(EditStockCategoryAction action, IDispatcher dispatcher)
    {
        var editedCategory = await _httpClient.PutAsJsonAsync($"api/StockCategories", action.Category);

        // Assuming your API returns the edited category
        //dispatcher.Dispatch(new StockCategoryEditedResultAction(editedCategory));
    }

    [EffectMethod]
    public async Task HandleRemoveStockCategoryAction(RemoveStockCategoryAction action, IDispatcher dispatcher)
    {
        await _httpClient.DeleteAsync($"api/StockCategories/{action.Sku}");

        // No need to return data on a delete, just dispatch a successful result
        //dispatcher.Dispatch(new StockCategoryRemovedResultAction(action.Sku));
    }
}

