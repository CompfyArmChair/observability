using Microsoft.AspNetCore.SignalR;

namespace ShopWebsite.Server.Hubs
{
	public class ShopHub : Hub
	{
		public async Task SendMessage(int basketId)
			=> await Clients.All.SendAsync("BasketChanged", basketId);
	}
}
