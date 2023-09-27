using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Shared.ServiceBus.Events;
using StockManagementWebsite.Server.Hubs;

namespace StockManagementWebsite.Server.EventConsumers;

public class CategoriesChangedEventConsumer : IConsumer<CategoriesChangedEvent>
{
	private readonly IHubContext<StockManagementHub> _hubContext;

	public CategoriesChangedEventConsumer(IHubContext<StockManagementHub> hubContext) 
		=> _hubContext = hubContext;

    public async Task Consume(ConsumeContext<CategoriesChangedEvent> context)
	{
		await _hubContext.Clients.All.SendAsync("CategoriesChanged");
	}
}
