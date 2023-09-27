using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Shared.ServiceBus.Events;
using StockManagementWebsite.Server.Hubs;

namespace StockManagementWebsite.Server.EventConsumers;

public class StockSkuChangedEventConsumer : IConsumer<StockSkuChangedEvent>
{
	private readonly IHubContext<StockManagementHub> _hubContext;

	public StockSkuChangedEventConsumer(IHubContext<StockManagementHub> hubContext)
		=> _hubContext = hubContext;

	public async Task Consume(ConsumeContext<StockSkuChangedEvent> context)
	{
		var message = context.Message;
		await _hubContext.Clients.All.SendAsync("StockSkuChanged", message.Sku);
	}
}
