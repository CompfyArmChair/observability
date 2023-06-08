using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Shared.ServiceBus.Commands;
using Shared.ServiceBus.Events;
using ShopWebsite.Server.Hubs;

namespace ShopWebsite.Server.EventConsumers;

public class BasketChangedEventConsumer : IConsumer<BasketChangedEvent>
{
	private readonly IHubContext<ShopHub> _hubContext;

	public BasketChangedEventConsumer(IHubContext<ShopHub> hubContext) 
		=> _hubContext = hubContext;

    public async Task Consume(ConsumeContext<BasketChangedEvent> context)
	{
		var message = context.Message;
		await _hubContext.Clients.All.SendAsync("BasketChanged", message.BasketId);
	}
}
