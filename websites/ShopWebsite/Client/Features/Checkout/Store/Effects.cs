using Fluxor;
using ShopWebsite.Client.Features.Checkout.Store.Actions;
using System.Net.Http.Json;

namespace ShopWebsite.Client.Features.Checkout.Store;

public class Effects
{
	private readonly HttpClient _httpClient;

	public Effects(HttpClient client) 
	{ 
		_httpClient = client;
	}

	[EffectMethod]
	public async Task CheckoutRequestAction(CheckoutRequestAction action, IDispatcher dispatcher)
	{
		await _httpClient.PostAsJsonAsync($"api/Purchase/Checkout", action.Purchase);
	}
}
