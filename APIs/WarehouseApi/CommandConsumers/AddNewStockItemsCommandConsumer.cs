using MassTransit;
using Shared.ServiceBus.Commands;
using Shared.ServiceBus.Events;
using WarehouseApi.Data;
using WarehouseApi.Data.Models;
using WarehouseApi.Enums;
using WarehouseApi.Instrumentation;

namespace WarehouseApi.CommandConsumers;

public class AddNewStockItemsCommandConsumer : IConsumer<AddNewStockItemsCommand>
{
    private readonly WarehouseDbContext _dbContext;
	private readonly OtelMeters _meters;

    public AddNewStockItemsCommandConsumer(WarehouseDbContext dbContext, OtelMeters meters)
    {
        _dbContext = dbContext;
		_meters = meters;
    }

    public async Task Consume(ConsumeContext<AddNewStockItemsCommand> context)
    {
		var message = context.Message;

		var newStockItems = new List<StockEntity>();
		var now = DateTime.Now;

        for (var i = 0; i < message.Quantity; i++)
		{
			newStockItems.Add(new() 
			{ 
				DateOfAddition = now,
				Sku = message.Sku,
				Status = Status.Available
			});
        }

		await _dbContext.AddRangeAsync(newStockItems);
		await _dbContext.SaveChangesAsync();

		await context.Publish(new StockSkuChangedEvent() { Sku = message.Sku });
		_meters.AddStock(message.Quantity);
		_meters.IncreaseTotalStock(message.Quantity);
	}
}
