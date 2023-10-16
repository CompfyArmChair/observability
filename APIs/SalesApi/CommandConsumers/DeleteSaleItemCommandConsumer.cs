using MassTransit;
using Microsoft.EntityFrameworkCore;
using SalesApi.Data;
using SalesApi.Instrumentation;
using Shared.ServiceBus.Commands;

namespace CatalogueApi.CommandConsumers;

public class DeleteSaleItemCommandConsumer : IConsumer<DeleteSaleItemCommand>
{
    private readonly SalesDbContext _dbContext;
	private readonly OtelMeters _meters;

	public DeleteSaleItemCommandConsumer(SalesDbContext dbContext, OtelMeters meters)
    {
        _dbContext = dbContext;
		_meters = meters;
    }

    public async Task Consume(ConsumeContext<DeleteSaleItemCommand> context)
    {
        var message = context.Message;

        var existingItem = await _dbContext.Products.SingleAsync(c => c.Sku == message.Sku);

        _dbContext.Remove(existingItem);
        await _dbContext.SaveChangesAsync();
        _meters.DeletePrice();
    }
}