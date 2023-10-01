using MassTransit;
using Microsoft.EntityFrameworkCore;
using SalesApi.Data;
using SalesApi.Instrumentation;
using Shared.ServiceBus.Commands;

namespace CatalogueApi.CommandConsumers;

public class EditSaleItemCommandConsumer : IConsumer<EditSaleItemCommand>
{
    private readonly SalesDbContext _dbContext;
	private readonly OtelMeters _meters;

	public EditSaleItemCommandConsumer(SalesDbContext dbContext, OtelMeters meters)
    {
        _dbContext = dbContext;
        _meters = meters;
    }

    public async Task Consume(ConsumeContext<EditSaleItemCommand> context)
    {
        var message = context.Message;

        var existingItem = await _dbContext.Products.SingleAsync(c => c.Sku == message.Sku);

        existingItem.Cost = message.Cost;

        await _dbContext.SaveChangesAsync();
        _meters.UpdatePrice();

	}
}
