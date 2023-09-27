using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.ServiceBus.Commands;
using Shared.ServiceBus.Events;
using WarehouseApi.Data;

namespace WarehouseApi.CommandConsumers;

public class DeleteStockItemCommandConsumer : IConsumer<DeleteStockItemCommand>
{
    private readonly WarehouseDbContext _dbContext;
    public DeleteStockItemCommandConsumer(WarehouseDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<DeleteStockItemCommand> context)
    {
        var message = context.Message;

		var toDelete = await _dbContext.Stock.SingleAsync(x => x.Id == message.Id);
       
        _dbContext.Remove(toDelete);
        await _dbContext.SaveChangesAsync();

		await context.Publish(new StockSkuChangedEvent() { Sku = toDelete.Sku });
	}
}
