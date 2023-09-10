using Fluxor;
using ShopWebsite.Client.Features.Products.Store.Actions;
using ShopWebsite.Shared;
using System.Net.Http.Json;

namespace ShopWebsite.Client.Features.Products.Store;

public class Effects
{
	private readonly HttpClient _httpClient;
    public Effects(HttpClient client) => _httpClient = client;

    [EffectMethod(typeof(FetchProductsAction))]
    public async Task HandleFetchProductsAction(IDispatcher dispatcher)
    {
        var products = await _httpClient.GetFromJsonAsync<IEnumerable<ProductDto>>("api/Products")
             ?? Enumerable.Empty<ProductDto>();

        dispatcher.Dispatch(new FetchProductsResultAction(products));
    }
}
