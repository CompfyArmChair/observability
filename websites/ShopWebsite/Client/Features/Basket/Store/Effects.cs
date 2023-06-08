using Fluxor;
using ShopWebsite.Client.Features.Basket.Store.Actions;
using ShopWebsite.Shared;
using System.Net.Http.Json;

namespace ShopWebsite.Client.Features.Basket.Store;

public class Effects
{
	private readonly HttpClient _httpClient;
	private readonly IState<BasketState> _basketState;
	public Effects(HttpClient client, IState<BasketState> basketState) 
	{ 
		_httpClient = client;
		_basketState = basketState;
	}

	[EffectMethod]
	public async Task AddToCurrentBasketRequestAction(AddToCurrentBasketRequestAction action, IDispatcher dispatcher)
	{
		await _httpClient.PostAsJsonAsync($"api/Basket/{_basketState.Value.BasketId}/Product", action.ProductToAdd);
	}

	[EffectMethod]
	public async Task HandleFetchBasketAction(FetchBasketAction action, IDispatcher dispatcher)
	{		
		var basket = await _httpClient.GetFromJsonAsync<BasketDto>($"api/Basket/{action.BasketId}");

		dispatcher.Dispatch(new FetchBasketResultAction(basket));
	}
}
