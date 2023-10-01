using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.ServiceBus.Commands;
using Shared.ServiceBus.Events;
using WarehouseApi.Data;
using WarehouseApi.Enums;
using WarehouseApi.Instrumentation;

namespace WarehouseApi.CommandConsumers;

public class DeleteStockItemCommandConsumer : IConsumer<DeleteStockItemCommand>
{
    private readonly WarehouseDbContext _dbContext;
	private readonly OtelMeters _meters;
	
    public DeleteStockItemCommandConsumer(WarehouseDbContext dbContext, OtelMeters meters)
    {
        _dbContext = dbContext;
        _meters = meters;
    }

    public async Task Consume(ConsumeContext<DeleteStockItemCommand> context)
    {
        var message = context.Message;

		var toDelete = await _dbContext.Stock.SingleAsync(x => x.Id == message.Id);
       
        _dbContext.Remove(toDelete);
        await _dbContext.SaveChangesAsync();

		await context.Publish(new StockSkuChangedEvent() { Sku = toDelete.Sku });
        _meters.DeleteStock();
		_meters.DecreaseTotalStock();
        if(toDelete.Status == Status.Sold)
        {
			_meters.DeleteSoldStock();
			_meters.DecreaseTotalSoldStock();
		}

		if (toDelete.Status == Status.Ordered)
		{
			_meters.DeleteReservedStock();
			_meters.DecreaseTotalReservedStock();
		}
	}
}
